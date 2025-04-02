using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class FeeScheduleEntryRepository : RepositoryAsync<ClientFeeScheduleEntry, int>, IFeeScheduleEntryRepository
	{
		private readonly ApplicationContext _dbContext;
		public FeeScheduleEntryRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ClientFeeScheduleEntry> GetById(int id)
		{
			return await _dbContext.ClientFeeScheduleEntries.Include(x => x.ClientCptCode)
														.Include(x => x.ClientFeeSchedule)
														.FirstOrDefaultAsync(c => c.Id == id) ?? new ClientFeeScheduleEntry();
		}


    }
}
