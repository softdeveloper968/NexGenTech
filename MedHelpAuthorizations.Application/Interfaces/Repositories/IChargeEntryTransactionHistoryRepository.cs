using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IChargeEntryTransactionHistoryRepository
    {

        IQueryable<ChargeEntryTransactionHistory> ChargeEntryTransactionHistories { get; }

        Task<List<ChargeEntryTransactionHistory>> GetListAsync();

        Task<ChargeEntryTransactionHistory> GetByIdAsync(int chargeEntryTransactionHistoryId);
        Task<ChargeEntryTransactionHistory> GetByChargeEntryTransactionIdAsync(int chargeEntryTransactionId);

        Task<int> InsertAsync(ChargeEntryTransactionHistory chargeEntryTransactionHistory);

        Task UpdateAsync(ChargeEntryTransactionHistory chargeEntryTransactionHistory);

        Task DeleteAsync(ChargeEntryTransactionHistory chargeEntryTransactionHistory);
    }
}
