using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    using MedHelpAuthorizations.Domain.IntegratedServices;
    using MedHelpAuthorizations.Infrastructure.Persistence.Context;

    public class ClaimStatusExceptionReasonCategoryMapRepository : RepositoryAsync<ClaimStatusExceptionReasonCategoryMap, int>, IClaimStatusExceptionReasonCategoryMapRepository
    {
		private readonly ApplicationContext _dbContext;

		public ClaimStatusExceptionReasonCategoryMapRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public IQueryable<ClaimStatusExceptionReasonCategoryMap> ClaimStatusExceptionReasonCategoryMaps => _dbContext.ClaimStatusExceptionReasonCategoryMaps;      

        public async Task<ClaimStatusExceptionReasonCategoryMap> GetByExceptionCategoryReasonAsync(string claimStatusExceptionCategoryReason)
        {
            return await ClaimStatusExceptionReasonCategoryMaps.Where(p => !string.IsNullOrWhiteSpace(p.ClaimStatusExceptionReasonText) && claimStatusExceptionCategoryReason.ToUpper().Trim().Contains(p.ClaimStatusExceptionReasonText.ToUpper().Trim())).FirstOrDefaultAsync();
        }
	}
}
