using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetRpaConfigurationsWithLocation;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientRpaCredentialConfigurationManager : IClientRpaCredentialConfigurationManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientRpaCredentialConfigurationManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> SaveAsync(CreateClientRpaCredentialConfigCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientRpaCredentialConfigurationEndPoints.CreateCredentialConfig, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateAsync(UpdateClientRpaCredentialConfigCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientRpaCredentialConfigurationEndPoints.UpdateCredentialConfig, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllClientRpaCredentialConfigurationsResponse>>> GetAllRpaCredentialConfigurations() //AA-23
        {
            var response = await _tenantHttpClient.GetAsync(ClientRpaCredentialConfigurationEndPoints.GetCredentialConfig);
            return await response.ToResult<List<GetAllClientRpaCredentialConfigurationsResponse>>();
        }


		public async Task<IResult<int>> ResetAlert(ResetClientRpaCredentialConfigCommand request) //AA-280
		{
			var response = await _tenantHttpClient.PostAsJsonAsync(ClientRpaCredentialConfigurationEndPoints.ResetCredentialConfig, request);
			return await response.ToResult<int>();
		}

        public async Task<IResult<int>> UpdateIsCredentialInUseAsync(UpdateIsCredentialInUseCommand request)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClientRpaCredentialConfigurationEndPoints.UpdateIsCredentialInUse(), request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetRpaConfigurationsWithLocationResponse>>> GetRpaConfigurationsWithLocationAsync() //EN-409
        {
            var response = await _tenantHttpClient.GetAsync(ClientRpaCredentialConfigurationEndPoints.GetRpaConfigurationsWithLocation);
            return await response.ToResult<List<GetRpaConfigurationsWithLocationResponse>>();
        }

    }
}
