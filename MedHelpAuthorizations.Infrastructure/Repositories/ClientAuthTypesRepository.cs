using MedHelpAuthorizations.Application.Interfaces.Repositories;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientAuthTypesRepositoryAsync : RepositoryAsync<ClientAuthType, int>, IClientAuthTypesRepository
    {
        private readonly IRepositoryAsync<ClientAuthType, int> _repository;

        private readonly ApplicationContext _dbContext;

        public ClientAuthTypesRepositoryAsync(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteAuthTypesForClient(int clientId)
        {
            _dbContext.ClientAuthTypes
                .RemoveRange(_dbContext.ClientAuthTypes
                    .Where(x => x.ClientId == clientId));
        }

        public Task<List<AuthType>> GetClientAuthTypes(int clientId)
        {
            throw new System.NotImplementedException();
        }
    }
}