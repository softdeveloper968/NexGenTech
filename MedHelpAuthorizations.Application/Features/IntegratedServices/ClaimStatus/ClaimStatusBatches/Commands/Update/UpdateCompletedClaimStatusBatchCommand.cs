using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Update
{
    public class UpdateCompletedClaimStatusBatchCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string AssignedToRpaCode { get; set; }
        public int NumberOfClaimsProcessed { get; set; }

        public class UpdateCompletedClaimStatusBatchCommandHandler : IRequestHandler<UpdateCompletedClaimStatusBatchCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
            private readonly IClaimStatusBatchHistoryRepository _claimStatusBatchHistoryRepository;
            private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
            private readonly IMapper _mapper;
            private readonly IClaimStatusEmailService _claimStatusEmailService;
            public UpdateCompletedClaimStatusBatchCommandHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IClaimStatusBatchHistoryRepository claimStatusBatchHistoryRepository, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IClaimStatusQueryService claimStatusQueryService, IClaimStatusEmailService claimStatusEmailService)
            {
                _claimStatusBatchRepository = claimStatusBatchRepository;
                _claimStatusBatchHistoryRepository = claimStatusBatchHistoryRepository;
                _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _claimStatusEmailService = claimStatusEmailService;
            }

            public async Task<Result<int>> Handle(UpdateCompletedClaimStatusBatchCommand command, CancellationToken cancellationToken)
            {
                ClaimStatusBatchHistory claimStatusBatchHistory;
                var claimStatusBatch = await _claimStatusBatchRepository.GetByIdAsync(command.Id);

                if (claimStatusBatch == null)
                {
                    return Result<int>.Fail($"TransactionBatch Not Found.");
                }
                else
                {
                    if (claimStatusBatch.CompletedDateTimeUtc != null)
                    {
                        return await Result<int>.FailAsync(
                            "Cannot mark the batch as completed because the batch is already marked as completed.");
                    }
                    else
                    {
                        claimStatusBatch.CompletedDateTimeUtc = DateTime.UtcNow;
                    }

                    var outstandingUnresolvedCount = await _claimStatusBatchClaimsRepository
                        .GetUnresolvedCountByBatchIdAsync(claimStatusBatch.Id).ConfigureAwait(true);
                    if (outstandingUnresolvedCount == 0)
                    {
                        claimStatusBatch.AllClaimStatusesResolvedOrExpired = true;
                    }

                    //if (command.NumberOfClaimsProcessed > 0)
                    //{
                    //    await _claimStatusEmailService.GetClaimCategoryCountsByBatchIdAndSendEmail(
                    //        claimStatusBatch.Id, command.AssignedToRpaCode, claimStatusBatch.CreatedOn.ToString("MM/dd/yyyy"));
                    //}

                    claimStatusBatch.Priority = null;

                    await _claimStatusBatchRepository.UpdateAsync(claimStatusBatch);

                    claimStatusBatchHistory = _mapper.Map<ClaimStatusBatchHistory>(claimStatusBatch);
                    claimStatusBatchHistory.DbOperationId = Domain.Entities.Enums.DbOperationEnum.Update;

                    await _claimStatusBatchHistoryRepository.InsertAsync(claimStatusBatchHistory);
                    await _unitOfWork.Commit(cancellationToken);

                    return Result<int>.Success(claimStatusBatch.Id);
                }
            }
        }
    }
}
