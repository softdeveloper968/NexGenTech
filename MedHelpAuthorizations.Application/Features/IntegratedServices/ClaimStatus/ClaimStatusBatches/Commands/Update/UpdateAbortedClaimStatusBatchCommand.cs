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
    public class UpdateAbortedClaimStatusBatchCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string AbortedReason { get; set; } = string.Empty;
        public bool ClearRpaProcesses { get; set; } = false;
        public string AssignedToRpaCode { get; set; }

        public class UpdateAbortedClaimStatusBatchCommandHandler : IRequestHandler<UpdateAbortedClaimStatusBatchCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
            private readonly IClaimStatusBatchHistoryRepository _claimStatusBatchHistoryRepository;
            private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
            private readonly IMapper _mapper;
            private readonly IClaimStatusEmailService _claimStatusEmailService;
            public UpdateAbortedClaimStatusBatchCommandHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IClaimStatusBatchHistoryRepository claimStatusBatchHistoryRepository, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IClaimStatusQueryService claimStatusQueryService, IClaimStatusEmailService claimStatusEmailService)
            {
                _claimStatusBatchRepository = claimStatusBatchRepository;
                _claimStatusBatchHistoryRepository = claimStatusBatchHistoryRepository;
                _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _claimStatusEmailService = claimStatusEmailService;
            }

            public async Task<Result<int>> Handle(UpdateAbortedClaimStatusBatchCommand command, CancellationToken cancellationToken)
            {
                var claimStatusBatch = await _claimStatusBatchRepository.GetByIdAsync(command.Id);

                if (claimStatusBatch == null)
                {
                    return Result<int>.Fail($"TransactionBatch Not Found.");
                }
                else
                {

                    if (claimStatusBatch.AbortedOnUtc != null)
                    {
                        return await Result<int>.FailAsync(
                            "Cannot mark the batch as aborted because the batch is already marked as aborted.");
                    }

                    claimStatusBatch.AbortedOnUtc = DateTime.UtcNow;
                    claimStatusBatch.AbortedReason = command.AbortedReason;

                    if (command.ClearRpaProcesses)
                    {
                        claimStatusBatch.AssignedToRpaLocalProcessIds = null;
                    }
                }

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

