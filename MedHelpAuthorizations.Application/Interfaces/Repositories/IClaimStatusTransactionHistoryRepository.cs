using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClaimStatusTransactionHistoryRepository
    {

        IQueryable<ClaimStatusTransactionHistory> ClaimStatusTransactionHistories { get; }

        Task<int> InsertAsync(ClaimStatusTransactionHistory claimStatusTransactionHistory);

    }
}
