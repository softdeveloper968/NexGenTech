using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ChargeEntryTransactionRepository : IChargeEntryTransactionRepository
    {
        private readonly IRepositoryAsync<ChargeEntryTransaction, int> _repository;

        public ChargeEntryTransactionRepository(IRepositoryAsync<ChargeEntryTransaction, int> repository)
        {
            _repository = repository;
        }

        public IQueryable<ChargeEntryTransaction> ChargeEntryTransactions => _repository.Entities;

        public async Task DeleteAsync(ChargeEntryTransaction chargeEntryTransaction)
        {
            await _repository.DeleteAsync(chargeEntryTransaction);
        }

        public async Task<List<ChargeEntryTransaction>> GetByBatchIdAsync(int chargeEntryBatchId)
        {
            return await _repository.Entities
                .Include(x => x.ChargeEntryTransactionHistories)
                .Where(p => p.ChargeEntryBatchId == chargeEntryBatchId)
                .ToListAsync();
        }
       
        public async Task<ChargeEntryTransaction> GetByIdAsync(int chargeEntryTransactionId)
        {
            return await _repository.Entities
                .Include(x => x.ChargeEntryTransactionHistories)
                .Where(p => p.Id == chargeEntryTransactionId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ChargeEntryTransaction>> GetListAsync()
        {
            return await _repository.Entities
                .Include(x => x.ChargeEntryTransactionHistories).
                ToListAsync();
        }

        public async Task<int> InsertAsync(ChargeEntryTransaction chargeEntryTransaction)
        {
            await _repository.AddAsync(chargeEntryTransaction);
            return chargeEntryTransaction.Id;
        }

        public async Task UpdateAsync(ChargeEntryTransaction chargeEntryTransaction)
        {
            await _repository.UpdateAsync(chargeEntryTransaction);
        }
    }
}
