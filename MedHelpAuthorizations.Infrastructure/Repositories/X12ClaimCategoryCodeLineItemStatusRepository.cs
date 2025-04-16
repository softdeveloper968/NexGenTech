using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class X12ClaimCategoryCodeLineItemStatusRepository : RepositoryAsync<X12ClaimCategoryCodeLineItemStatus, int>, IX12ClaimCategoryCodeLineItemStatusRepository
	{
		private readonly ApplicationContext _dbContext;
		public X12ClaimCategoryCodeLineItemStatusRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public IQueryable<X12ClaimCategoryCodeLineItemStatus> X12ClaimCategoryCodeLineItemStatuses => _dbContext.X12ClaimCategoryCodeLineItemStatuses;

		public async Task<List<X12ClaimCategoryCodeLineItemStatus>> GetAllForClaimLineItemStatusId(ClaimLineItemStatusEnum claimLineItemStatusId)
		{
			return await _dbContext.X12ClaimCategoryCodeLineItemStatuses
							  .Where(x => x.ClaimLineItemStatusId == claimLineItemStatusId)							  
							  .ToListAsync();
		}

		public async Task<X12ClaimCategoryCodeLineItemStatus> GetByX12ClaimCategoryCodeString(string x12ClaimCategoryCode)
		{
			x12ClaimCategoryCode = x12ClaimCategoryCode.ToUpper().Trim() ?? string.Empty;

			return await _dbContext.X12ClaimCategoryCodeLineItemStatuses
					.Include(x => x.ClaimLineItemStatus)
					.Include(x => x.X12ClaimCategory)
					.FirstOrDefaultAsync(x => x.Code.ToUpper().Trim() == x12ClaimCategoryCode.ToUpper().Trim());
		}
	}
}
