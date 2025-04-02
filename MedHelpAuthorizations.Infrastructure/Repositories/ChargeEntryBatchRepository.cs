using System;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ChargeEntryBatchRepository : IChargeEntryBatchRepository
    {
        private readonly IRepositoryAsync<ChargeEntryBatch, int> _repository;

        public ChargeEntryBatchRepository(IRepositoryAsync<ChargeEntryBatch, int> repository)
        {
            _repository = repository;
        }

        public IQueryable<ChargeEntryBatch> ChargeEntryBatches => _repository.Entities;

        public async Task DeleteAsync(ChargeEntryBatch claimStatusBatch)
        {
            await _repository.DeleteAsync(claimStatusBatch);
        }

        public Task<List<ChargeEntryBatch>> GetAllUnprocessedAsync()
        {
            throw new NotImplementedException();
        }

        /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
        public async Task<ChargeEntryBatch> GetByIdAsync(int chargeEntryBatchId)
        {
            return await this._repository.Entities
                       .Include(x => x.ChargeEntryRpaConfiguration)
                       .ThenInclude(y => y.Client)
                       .Include(x => x.ChargeEntryRpaConfiguration)
                        .ThenInclude(y => y.AuthType)
                       .Include(x => x.ChargeEntryRpaConfiguration)
                       .ThenInclude(y => y.RpaType)
                       .Include(x => x.ChargeEntryRpaConfiguration)
                       .ThenInclude(y => y.TransactionType)
                       .Where(x => x.Id == chargeEntryBatchId)
                       .FirstOrDefaultAsync().ConfigureAwait(true);
        }

        public async Task<List<ChargeEntryBatch>> GetListAsync()
        {
            return await this._repository.Entities
                       .Include(x => x.ChargeEntryRpaConfiguration)
                       .ThenInclude(y => y.Client)
                       .Include(x => x.ChargeEntryRpaConfiguration)
                       .ThenInclude(y => y.AuthType)
                       .Include(x => x.ChargeEntryRpaConfiguration)
                       .ThenInclude(y => y.RpaType)
                       .Include(x => x.ChargeEntryRpaConfiguration)
                       .ThenInclude(y => y.TransactionType)
                       .ToListAsync().ConfigureAwait(true);

            return null;
        }

        public Task<List<ChargeEntryBatch>> GetCompletedByRpaTypeAsync(RpaTypeEnum rpaType)
        {
            throw new NotImplementedException();
        }       

        public Task<List<ChargeEntryBatch>> GetNotCompletedByRpaTypeAsync(RpaTypeEnum rpaType)
        {
            throw new NotImplementedException();
        }

        public Task<List<ChargeEntryBatch>> GetAllCompletedAsync(RpaTypeEnum rpaType)
        {
            throw new NotImplementedException();
        }

        public Task<List<ChargeEntryBatch>> GetAllNotCompletedAsync(RpaTypeEnum rpaType)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync(ChargeEntryBatch claimStatusBatch)
        {
            await _repository.AddAsync(claimStatusBatch);
            return claimStatusBatch.Id;
        }

        public async Task UpdateAsync(ChargeEntryBatch claimStatusBatch)
        {
            await _repository.UpdateAsync(claimStatusBatch);
        }
    }
}
