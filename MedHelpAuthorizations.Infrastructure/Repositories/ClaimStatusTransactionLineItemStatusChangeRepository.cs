using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClaimStatusTransactionLineItemStatusChangeRepository : RepositoryAsync<ClaimStatusTransactionLineItemStatusChangẹ, int>, IClaimStatusTransactionLineItemStatusChangeRepository
    {
        private readonly ApplicationContext _dbContext;
        public ClaimStatusTransactionLineItemStatusChangeRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ClaimStatusTransactionLineItemStatusChangẹ> ClaimStatusTransactionLineItemStatusChangẹs => _dbContext.ClaimStatusTransactionLineItemStatusChangẹs;

        public async Task DeleteAsync(ClaimStatusTransactionLineItemStatusChangẹ claimStatusTransactionLineItemStatusChangẹ)
        {
            _dbContext.Remove(claimStatusTransactionLineItemStatusChangẹ);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ClaimStatusTransactionLineItemStatusChangẹ> GetByIdAsync(int id)
        {
            return await _dbContext.ClaimStatusTransactionLineItemStatusChangẹs
                       .Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        //public async Task<ClaimStatusTransactionLineItemStatusChangẹ> GetByClaimStatusTransactionIdAsync(int claimStatusTractionId)
        //{
        //    return await _repository.Entities
        //               .Where(p => p.ClaimStatusTransactionId == claimStatusTractionId)
        //               .FirstOrDefaultAsync();
        //}

        public async Task<List<ClaimStatusTransactionLineItemStatusChangẹ>> GetListAsync()
        {
            return await _dbContext.ClaimStatusTransactionLineItemStatusChangẹs.ToListAsync();
        }

        public async Task<int> InsertAsync(ClaimStatusTransactionLineItemStatusChangẹ claimStatusTransactionLineItemStatusChangẹ)
        {
            await _dbContext.ClaimStatusTransactionLineItemStatusChangẹs.AddAsync(claimStatusTransactionLineItemStatusChangẹ);
            return claimStatusTransactionLineItemStatusChangẹ.Id;
        }
        public async Task<ClaimLineItemStatus> GetClaimLineItemStatusIdRankAsync(int claimLineItemStatusId)
        {
            return await _dbContext.ClaimLineItemStatuses
                .Where(r => r.Id == (ClaimLineItemStatusEnum)claimLineItemStatusId)
                .FirstOrDefaultAsync();
        }
        public async Task UpdateAsync(ClaimStatusTransactionLineItemStatusChangẹ claimStatusTransactionLineItemStatusChangẹ)
        {
            _dbContext.Update(claimStatusTransactionLineItemStatusChangẹ);
            await _dbContext.SaveChangesAsync();
        }
    }
}
