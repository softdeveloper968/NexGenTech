using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClaimStatusTransactionLineItemStatusChangeRepository : IRepositoryAsync<ClaimStatusTransactionLineItemStatusChangẹ, int>
    {
        //TAPI-118
        IQueryable<ClaimStatusTransactionLineItemStatusChangẹ> ClaimStatusTransactionLineItemStatusChangẹs { get; }

        Task<List<ClaimStatusTransactionLineItemStatusChangẹ>> GetListAsync();

        Task<ClaimStatusTransactionLineItemStatusChangẹ> GetByIdAsync(int id);

        //Task<ClaimStatusTransactionLineItemStatusChangẹ> GetByClaimStatusTransactionIdAsync(int claimStatusTractionId);

        Task<int> InsertAsync(ClaimStatusTransactionLineItemStatusChangẹ claimStatusTransactionLineItemStatusChangẹ);

        Task UpdateAsync(ClaimStatusTransactionLineItemStatusChangẹ claimStatusTransactionLineItemStatusChangẹ);

        Task DeleteAsync(ClaimStatusTransactionLineItemStatusChangẹ claimStatusTransactionLineItemStatusChangẹ);
        //Task<ClaimLineItemStatus> GetClaimLineItemStatusIdRankAsync(int claimLineItemStatusId);
    }
}
