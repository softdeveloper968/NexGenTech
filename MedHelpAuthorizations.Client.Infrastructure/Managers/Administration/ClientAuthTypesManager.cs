using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetByClientId;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
	public class ClientAuthTypesManager : IClientAuthTypesManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientAuthTypesManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<GetClientAuthTypesByClientIdResponse>>> GetByClientIdAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ClientAuthTypeEndpoint.GetByClientId());
            return await response.ToResult<List<GetClientAuthTypesByClientIdResponse>>();
        }
        public async Task<IResult<List<GetClientLocationServiceTypesResponse>>> GetClientLocationServiceTypes(int locationId)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.ClientAuthTypeEndpoint.GetClientLocationServiceTypes(locationId));
            return await response.ToResult<List<GetClientLocationServiceTypesResponse>>();
        }
    }
}
