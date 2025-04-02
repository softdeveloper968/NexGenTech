using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClaimLineItemStatusRepository : RepositoryAsync<ClaimLineItemStatus, ClaimLineItemStatusEnum>, IClaimLineItemStatusRepository
    {
		private readonly ApplicationContext _dbContext;

		public ClaimLineItemStatusRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

        public async Task<List<ClaimLineItemStatus>> GetListAsync()
        {
            return await _dbContext.ClaimLineItemStatuses
                .ToListAsync(); 
        }

    //    public async Task<ClaimLineItemStatus> GetByIdAsync(ClaimLineItemStatusEnum id)
    //    {
    //        return await _dbContext.ClaimLineItemStatuses
				//.Where(r => r.Id == id)
    //            .FirstOrDefaultAsync();
    //    }
    }
}
