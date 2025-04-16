using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class UnMappedFeeScheduleCptRepository : RepositoryAsync<UnmappedFeeScheduleCpt, int>, IUnMappedFeeScheduleCptRepository
	{
		private readonly ApplicationContext _dbContext;

		public UnMappedFeeScheduleCptRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<List<UnmappedFeeScheduleCpt>> GetByClientId(int clientId)
		{
			return await _dbContext.UnmappedFeeScheduleCpts
				   .Where(x => x.ClientId == clientId)
				   .ToListAsync() ?? new List<UnmappedFeeScheduleCpt>();
		}

        public async Task<UnmappedFeeScheduleCpt> GetByCriteria(int clientCptCodeId, int clientInsuranceId, int dateOfService, int clientId)
        {
            return await _dbContext.UnmappedFeeScheduleCpts
                .Where(x => x.ClientId == clientId
                         && x.ClientCptCodeId == clientCptCodeId
                         && x.ClientInsuranceId == clientInsuranceId
                         && x.DateOfServiceYear == dateOfService)
                .FirstOrDefaultAsync() ?? new UnmappedFeeScheduleCpt();
        }

    }
}
