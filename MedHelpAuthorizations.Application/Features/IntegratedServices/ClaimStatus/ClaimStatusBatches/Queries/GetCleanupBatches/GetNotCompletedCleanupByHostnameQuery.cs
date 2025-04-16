using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Responses.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetCleanupBatches
{
    public class GetNotCompletedCleanupByHostnameQuery : IRequest<Result<List<ClaimStatusBatchLastTransactionResponse>>>
    {
        public string HostName = string.Empty;
    }

    public class GetNotCompletedCleanupByHostnameQueryHandler : IRequestHandler<GetNotCompletedCleanupByHostnameQuery, Result<List<ClaimStatusBatchLastTransactionResponse>>>
    {
        private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
        private readonly IClaimStatusTransactionRepository _claimStatusTransactionRepository;

        public GetNotCompletedCleanupByHostnameQueryHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IClaimStatusTransactionRepository claimStatusTransactionRepository)
        {
            _claimStatusBatchRepository = claimStatusBatchRepository;
            _claimStatusTransactionRepository = claimStatusTransactionRepository;   
        }

        public async Task<Result<List<ClaimStatusBatchLastTransactionResponse>>> Handle(GetNotCompletedCleanupByHostnameQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchCleanupList = await _claimStatusBatchRepository.GetNotCompletedCleanupByHostName(query.HostName);

            var lastTransactionInfo = claimStatusBatchCleanupList.Select(b =>
                new ClaimStatusBatchLastTransactionResponse()
                {
                    ClaimStatusBatchId = b.Id,
                    AssignedToRpaLocalProcessIds = b.AssignedToRpaLocalProcessIds
                })
                .ToList();

            foreach (var lt in lastTransactionInfo)
            {
                var transaction = await _claimStatusTransactionRepository
                    .GetLastTransactionByBatchIdAsync(lt.ClaimStatusBatchId);

                lt.LastClaimStatusBatchTransactionDate = transaction?.ClaimStatusTransactionEndDateTimeUtc ?? DateTime.MinValue;
            }

            return await Result<List<ClaimStatusBatchLastTransactionResponse>>.SuccessAsync(lastTransactionInfo);
        }
    }
}
