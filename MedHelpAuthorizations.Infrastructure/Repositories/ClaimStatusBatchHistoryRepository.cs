using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClaimStatusBatchHistoryRepository : IClaimStatusBatchHistoryRepository
    {
        private readonly IRepositoryAsync<ClaimStatusBatchHistory, int> _repository;

        public ClaimStatusBatchHistoryRepository(IRepositoryAsync<ClaimStatusBatchHistory, int> repository)
        {
            _repository = repository;
        }

        public IQueryable<ClaimStatusBatchHistory> ClaimStatusBatchHistories => _repository.Entities;

        public async Task DeleteAsync(ClaimStatusBatchHistory claimStatusBatchHistory)
        {
            await _repository.DeleteAsync(claimStatusBatchHistory);
        }

        public async Task<List<ClaimStatusBatchHistory>> GetByClaimStatusBatchIdAsync(int ClaimStatusBatchId)
        {
            return await _repository.Entities
                            .Include(x => x.ClaimStatusBatch)
                            .Include(x => x.ClientInsurance)
                                .ThenInclude(x => x.RpaInsurance)
                            .Include(x => x.ClientInsurance.Client)
                            .Where(x => x.ClaimStatusBatchId == ClaimStatusBatchId)
                            .ToListAsync();
        }

        public async Task<ClaimStatusBatchHistory> GetByIdAsync(int claimStatusBatchHistoryId)
        {
            return await _repository.Entities
                .Include(x => x.ClaimStatusBatch)
                .Include(x => x.ClientInsurance)
                    .ThenInclude(x => x.RpaInsurance)
                .Include(x => x.ClientInsurance.Client)
                .Where(p => p.Id == claimStatusBatchHistoryId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ClaimStatusBatchHistory>> GetListAsync()
        {
            return await _repository.Entities
                 .Include(x => x.ClaimStatusBatch)
                .Include(x => x.ClientInsurance)
                    .ThenInclude(x => x.RpaInsurance)
                .Include(x => x.ClientInsurance.Client)
                .ToListAsync();
        }

        public async Task<int> InsertAsync(ClaimStatusBatchHistory claimStatusBatchHistory)
        {
            await _repository.AddAsync(claimStatusBatchHistory);
            return claimStatusBatchHistory.Id;
        }

        public async Task UpdateAsync(ClaimStatusBatchHistory claimStatusBatchHistory)
        {
            await _repository.UpdateAsync(claimStatusBatchHistory);
        }
    }
}
