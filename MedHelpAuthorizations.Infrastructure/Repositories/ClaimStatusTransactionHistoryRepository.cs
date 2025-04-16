using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClaimStatusTransactionHistoryRepository : RepositoryAsync<ClaimStatusTransactionHistory, int>, IClaimStatusTransactionHistoryRepository
    {
		private readonly ApplicationContext _dbContext;

		public ClaimStatusTransactionHistoryRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

        public IQueryable<ClaimStatusTransactionHistory> ClaimStatusTransactionHistories => _dbContext.ClaimStatusTransactionHistories;

        public async Task<List<ClaimStatusTransactionHistory>> GetListAsync()
        {
            return await ClaimStatusTransactionHistories.ToListAsync();
        }

        public async Task<int> InsertAsync(ClaimStatusTransactionHistory claimStatusTransactionHistory)
        {
            await _dbContext.AddAsync(claimStatusTransactionHistory);
            return claimStatusTransactionHistory.Id;
        }
    }
}
