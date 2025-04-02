using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ExceptionReason.Queries;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClaimStatusTransactionRepository : RepositoryAsync<ClaimStatusTransaction, int>, IClaimStatusTransactionRepository
	{
		private readonly ApplicationContext _dbContext;

		public ClaimStatusTransactionRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
		public ClaimStatusTransactionRepository(ApplicationContext dbContext, ITenantInfo tenantInfo) : base(dbContext, tenantInfo)
		{
			_dbContext = dbContext;
		}

		//private readonly IRepositoryAsync<ClaimStatusTransaction, int> _repository;

		//public ClaimStatusTransactionRepository(IRepositoryAsync<ClaimStatusTransaction, int> repository)
		//{
		//    _repository = repository;
		//}

		public IQueryable<ClaimStatusTransaction> ClaimStatusTransactions => _dbContext.ClaimStatusTransactions;

        //public async Task DeleteAsync(ClaimStatusTransaction ClaimStatusTransaction)
        //{
        //    _dbContext.ClaimStatusTransactions.Remove(ClaimStatusTransaction);
        //    await _dbContext.SaveChangesAsync();
        //}

        public new async Task<ClaimStatusTransaction> GetByIdAsync(int id)
        {
            return await _dbContext.ClaimStatusTransactions
							.Include(x => x.ClaimLineItemStatus)
							.Where(x => x.Id == id)
							.FirstOrDefaultAsync();
        }


  //      public async Task<ClaimStatusTransaction> GetByBatchClaimIdAsync(int claimStatusBatchClaimId)
		//{
		//	return await _dbContext.ClaimStatusTransactions
		//		.Include(x => x.ClaimStatusTransactionHistories)
		//		.Where(p => p.ClaimStatusBatchClaimId == claimStatusBatchClaimId).FirstOrDefaultAsync();
		//}

		public async Task<ClaimStatusTransaction> GetLastTransactionByBatchIdAsync(int claimStatusBatchId)
		{
			return await _dbContext.ClaimStatusTransactions
				.Include(t => t.ClaimStatusBatchClaim)
				.Where(t => t.ClaimStatusBatchClaim.ClaimStatusBatchId == claimStatusBatchId)
				.OrderByDescending(t => t.ClaimStatusTransactionEndDateTimeUtc)
				.FirstOrDefaultAsync();
		}

		//      public async Task<ClaimStatusTransaction> GetByIdAsync(int claimStatusTransactionId)
		//      {
		//	try
		//	{
		//		return await _dbContext.ClaimStatusTransactions
		//			   //.Include(x => x.ClaimStatusTransactionLineItemStatusChangẹ)
		//			   //.Include(x => x.ClaimStatusTransactionHistories)
		//			   //.Include(x => x.ClaimLineItemStatus)
		//				.FirstOrDefaultAsync(p => p.Id == claimStatusTransactionId);
		//	}
		//	catch (System.Exception ex)
		//	{

		//		throw ex;
		//	}
		//}

		public async Task<List<ClaimStatusTransaction>> GetListAsync()
		{
			return await _dbContext.ClaimStatusTransactions.Include(x => x.ClaimStatusTransactionHistories).ToListAsync();
		}

		public async Task<int> InsertAsync(ClaimStatusTransaction ClaimStatusTransaction)
		{
			await _dbContext.ClaimStatusTransactions.AddAsync(ClaimStatusTransaction);
			return ClaimStatusTransaction.Id;
		}

		public async Task UpdateAsync(ClaimStatusTransaction ClaimStatusTransaction)
		{
			_dbContext.ClaimStatusTransactions.Update(ClaimStatusTransaction);
			await _dbContext.SaveChangesAsync();
		}


    }
}
