using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public partial class ClaimStatusQueryService
    {
        public async Task<int> UnassignClaimStatusBatchAsync(int batchId, string tenantIdentifier = null)
        {   
            var cancellationToken = new CancellationToken();
            if (!string.IsNullOrWhiteSpace(tenantIdentifier))
            {
                _unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);
            }

            ClaimStatusBatchHistory claimStatusBatchHistory;
            var claimStatusBatch = await _unitOfWork.Repository<ClaimStatusBatch>().GetByIdAsync(batchId);

            claimStatusBatch.AssignedDateTimeUtc = null;
            claimStatusBatch.AssignedToIpAddress = null;
            claimStatusBatch.AssignedToHostName = null;
            claimStatusBatch.AssignedToRpaCode = null;
            claimStatusBatch.AssignedToRpaLocalProcessIds = null;
            claimStatusBatch.AssignedClientRpaConfigurationId = null;
            claimStatusBatch.AssignedToRpaLocalProcessIds = null;
            claimStatusBatch.CompletedDateTimeUtc = null;
            claimStatusBatch.AbortedOnUtc = null;
            claimStatusBatch.AbortedReason = null;
            claimStatusBatch.Priority = null;

            claimStatusBatchHistory = _mapper.Map<ClaimStatusBatchHistory>(claimStatusBatch);
            claimStatusBatchHistory.ClaimStatusBatchId = claimStatusBatch.Id;
            claimStatusBatchHistory.DbOperationId = DbOperationEnum.Update;

            await _unitOfWork.Repository<ClaimStatusBatchHistory>().AddAsync(claimStatusBatchHistory).ConfigureAwait(true);
            await _unitOfWork.Repository<ClaimStatusBatch>().UpdateAsync(claimStatusBatch);

            return await _unitOfWork.Commit(cancellationToken); 
        }
    }
}