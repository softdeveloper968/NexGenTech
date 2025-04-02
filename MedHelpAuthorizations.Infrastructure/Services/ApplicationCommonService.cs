using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ApplicationCommonService : IApplicationCommonService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public ApplicationCommonService(ICurrentUserService currentUserService, ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _currentUserService = currentUserService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<Result<string[]>> GetApplicationFeatures()
        {
            IClientRepository clientRepository = _tenantRepositoryFactory.GetClientRepository(_currentUserService.TenantIdentifier);
            var client = await clientRepository.GetById(_currentUserService.ClientId);
            var res = client.ClientApplicationFeatures.Select(x => x.ApplicationFeatureId.ToString()).ToArray();
            return Result<string[]>.Success(res);
        }
        public async Task<Result<ApiIntegrationKey[]>> GetApiKeys()
        {
            IClientRepository clientRepository = _tenantRepositoryFactory.GetClientRepository(_currentUserService.TenantIdentifier);
            var client = await clientRepository.GetById(_currentUserService.ClientId);
            var res = client.ClientApiIntegrationKeys.Select(x => new ApiIntegrationKey() { ApiIntegrationName = x.ApiIntegrationId.ToString(), ApiKey = x.ApiKey }).ToArray();
            return Result<ApiIntegrationKey[]>.Success(res);
        }
    }
}
