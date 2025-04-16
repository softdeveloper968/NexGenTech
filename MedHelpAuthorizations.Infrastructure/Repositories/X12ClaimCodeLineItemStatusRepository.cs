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
    public class X12ClaimCodeLineItemStatusRepository : RepositoryAsync<X12ClaimCodeLineItemStatus, int>, IX12ClaimCodeLineItemStatusRepository
	{
		private readonly ApplicationContext _dbContext;
		public X12ClaimCodeLineItemStatusRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public IQueryable<X12ClaimCodeLineItemStatus> X12ClaimCodeLineItemStatuses => _dbContext.X12ClaimCodeLineItemStatuses;

		public async Task<List<X12ClaimCodeLineItemStatus>> GetAllForClaimLineItemStatusId(ClaimLineItemStatusEnum claimLineItemStatusId)
		{
			return await _dbContext.X12ClaimCodeLineItemStatuses
							.Include(x => x.ClaimLineItemStatus)
							.Include(x => x.X12ClaimCodeType)
							  .Where(x => x.ClaimLineItemStatusId == claimLineItemStatusId)
							  .ToListAsync();
		}

		public async Task<X12ClaimCodeLineItemStatus> GetByX12ClaimCodeString(string x12ClaimCode)
		{
			x12ClaimCode = x12ClaimCode.ToUpper().Trim() ?? string.Empty;

			return await _dbContext.X12ClaimCodeLineItemStatuses
					.Include(x => x.ClaimLineItemStatus)
					.Include(x => x.X12ClaimCodeType)
					.FirstOrDefaultAsync(x => x.Code.ToUpper().Trim() == x12ClaimCode.ToUpper().Trim());
		}

		public async Task<List<X12ClaimCodeLineItemStatus>> GetListByX12DelimitedClaimCodeString(string commaDelimitedClaimCodes)
		{
			commaDelimitedClaimCodes = commaDelimitedClaimCodes?.Replace(" ", "")?.ToUpper()?.Trim() ?? string.Empty;
			var x12ClaimCodes = commaDelimitedClaimCodes.Split(','); //.ToList();

			return await _dbContext.X12ClaimCodeLineItemStatuses
					.Include(x => x.ClaimLineItemStatus)
					.Include(x => x.X12ClaimCodeType)
					.Where(x => x12ClaimCodes.Contains(x.Code.ToUpper().Trim())).ToListAsync();
		}
	}
}
