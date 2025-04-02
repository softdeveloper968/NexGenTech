using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ProviderRepositoryAsync : RepositoryAsync<ClientProvider, int>, IProviderRepository
    {
        private readonly ApplicationContext _dbContext;

        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public ProviderRepositoryAsync(ApplicationContext dbContext, ICurrentUserService currentUserService) : base(dbContext)
        {
            _dbContext = dbContext; 
            _currentUserService = currentUserService;
        }

        public IQueryable<ClientProvider> Providers => _dbContext.ClientProviders;

        public async Task<ClientProvider> FindByNpiAsync(string npi)
        {
            return await Providers
               .Specify(new GenericByClientIdSpecification<ClientProvider>(_clientId))
               .Where(p => p.Npi == npi)
               .FirstOrDefaultAsync();
        }
    }
}
