using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IChargeEntryTransactionRepository
    {
        IQueryable<ChargeEntryTransaction> ChargeEntryTransactions { get; }

        Task<List<ChargeEntryTransaction>> GetListAsync();

        Task<ChargeEntryTransaction> GetByIdAsync(int Id);

        Task<int> InsertAsync(ChargeEntryTransaction chargeEntryTransaction);

        Task UpdateAsync(ChargeEntryTransaction chargeEntryTransaction);

        Task DeleteAsync(ChargeEntryTransaction chargeEntryTransaction);

        Task<List<ChargeEntryTransaction>> GetByBatchIdAsync(int chargeEntryBatchId);
    }    
}
