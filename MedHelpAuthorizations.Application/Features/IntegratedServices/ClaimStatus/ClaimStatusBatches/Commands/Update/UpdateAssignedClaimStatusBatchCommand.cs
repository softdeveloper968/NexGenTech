using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Update
{
    public class UpdateAssignedClaimStatusBatchCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int? AssignedClientRpaConfigurationId { get; set; }
        public string AssignedToRpaCode { get; set; }
        public string AssignedToIpAddress { get; set; }
        public string AssignedToHostName { get; set; }
        public string AssignedToRpaProcessIds { get; set; }
        public class UpdateAssignedClaimStatusBatchCommandHandler : IRequestHandler<UpdateAssignedClaimStatusBatchCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
            private readonly IClaimStatusBatchHistoryRepository _claimStatusBatchHistoryRepository;
            private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
            private readonly IMapper _mapper;
            private readonly IClaimStatusEmailService _claimStatusEmailService;
            public UpdateAssignedClaimStatusBatchCommandHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IClaimStatusBatchHistoryRepository claimStatusBatchHistoryRepository, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IClaimStatusQueryService claimStatusQueryService, IClaimStatusEmailService claimStatusEmailService)
            {
                _claimStatusBatchRepository = claimStatusBatchRepository;
                _claimStatusBatchHistoryRepository = claimStatusBatchHistoryRepository;
                _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _claimStatusEmailService = claimStatusEmailService;
            }

            public async Task<Result<int>> Handle(UpdateAssignedClaimStatusBatchCommand command, CancellationToken cancellationToken)
            {
                if (command.AssignedToRpaCode == null)
                {
                    return Result<int>.Fail($"RPA code not sent as parameters");
                }
                var claimStatusBatch = await _claimStatusBatchRepository.GetByIdAsync(command.Id);

                if (claimStatusBatch == null)
                {
                    return Result<int>.Fail($"TransactionBatch Not Found.");
                }
                if (claimStatusBatch.AssignedDateTimeUtc != null)
                {
                    return await Result<int>.FailAsync("Cannot complete the batch assignment request because the batch has already been assigned.");
                }

                if (command.AssignedClientRpaConfigurationId == null ||
                    command.AssignedClientRpaConfigurationId == 0)
                {
                    return await Result<int>.FailAsync("The bot claiming assignment must send the ClientRpaInsuranceConfigurationId of the configuration it is using.");
                }

                claimStatusBatch.AssignedDateTimeUtc = DateTime.UtcNow;
                claimStatusBatch.AssignedToIpAddress = command.AssignedToIpAddress;
                claimStatusBatch.AssignedToHostName = command.AssignedToHostName;
                claimStatusBatch.AssignedToRpaCode = command.AssignedToRpaCode;
                claimStatusBatch.AssignedToRpaLocalProcessIds = command.AssignedToRpaProcessIds;
                claimStatusBatch.AssignedClientRpaConfigurationId = command.AssignedClientRpaConfigurationId;

                claimStatusBatch.Priority = null;

                await _claimStatusBatchRepository.UpdateAsync(claimStatusBatch);

                var claimStatusBatchHistory = _mapper.Map<ClaimStatusBatchHistory>(claimStatusBatch);
                claimStatusBatchHistory.DbOperationId = Domain.Entities.Enums.DbOperationEnum.Update;

                await _claimStatusBatchHistoryRepository.InsertAsync(claimStatusBatchHistory);
                await _unitOfWork.Commit(cancellationToken);

                return Result<int>.Success(claimStatusBatch.Id);
            }
        }
    }
}

