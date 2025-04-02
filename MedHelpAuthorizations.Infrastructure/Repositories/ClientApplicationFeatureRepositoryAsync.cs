using MedHelpAuthorizations.Application.Interfaces.Repositories;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientApplicationFeatureRepositoryAsync : RepositoryAsync<ClientApplicationFeature, int>, IClientApplicationFeatureRepository
    {
        private readonly IRepositoryAsync<ClientApplicationFeature, int> _repository;

        private readonly ApplicationContext _dbContext;

        public ClientApplicationFeatureRepositoryAsync(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task DeleteApplicationFeaturesForClient(int clientId)
        {
            _dbContext.ClientApplicationFeatures
                .RemoveRange(_dbContext.ClientApplicationFeatures
                    .Where(x => x.ClientId == clientId));
        }

        public Task<List<ApplicationFeature>> GetClientApplicationFeatures(int clientId)
        {
            throw new System.NotImplementedException();
        }
    }
}