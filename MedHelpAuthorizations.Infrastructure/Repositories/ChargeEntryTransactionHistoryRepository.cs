using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ChargeEntryTransactionHistoryRepository : IChargeEntryTransactionHistoryRepository
    {
        public IQueryable<ChargeEntryTransactionHistory> ChargeEntryTransactionHistories => throw new NotImplementedException();

        public Task DeleteAsync(ChargeEntryTransactionHistory chargeEntryTransactionHistory)
        {
            throw new NotImplementedException();
        }

        public Task<ChargeEntryTransactionHistory> GetByChargeEntryTransactionIdAsync(int chargeEntryTransactionId)
        {
            throw new NotImplementedException();
        }

        public Task<ChargeEntryTransactionHistory> GetByIdAsync(int chargeEntryTransactionHistoryId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ChargeEntryTransactionHistory>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(ChargeEntryTransactionHistory chargeEntryTransactionHistory)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ChargeEntryTransactionHistory chargeEntryTransactionHistory)
        {
            throw new NotImplementedException();
        }
    }
}
