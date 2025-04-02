using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientCptCodeRepository : RepositoryAsync<ClientCptCode, int>, IClientCptCodeRepository
	{
		private readonly ApplicationContext _dbContext;
		public ClientCptCodeRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<ClientCptCode> GetByClientId(int id, string code)
		{
			return await _dbContext.ClientCptCodes
				.FirstOrDefaultAsync(c => c.ClientId == id && c.Code == code);
		}
		
    }
}
