using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Queries.GetByLocationId;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Commands.AddEdit;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientLocationInsuranceIdentifierManager : IClientLocationInsuranceIdentifierManager
    { 
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientLocationInsuranceIdentifierManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientLocationInsuranceIdentifierEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetClientLocationInsuranceIdentifierByLocationIdResponse>>> GetAllByClientLocationIdAsync(int locationId)
        {
            var response = await _tenantHttpClient.GetAsync(ClientLocationInsuranceIdentifierEndpoints.GetAllByClientLocationId(locationId));
            return await response.ToResult<List<GetClientLocationInsuranceIdentifierByLocationIdResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientLocationInsuranceIdentifierCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationInsuranceIdentifierEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}
