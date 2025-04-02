using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Create;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByRpaInsurance;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByUserrnameAndUrl;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.ClaimStatus
{
    public class ClientInsuranceRpaConfigurationManager : IClientInsuranceRpaConfigurationManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientInsuranceRpaConfigurationManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<IResult<List<GetAllClientInsuranceRpaConfigurationsResponse>>> GetAllAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsuranceRpaConfigurationEndpoint.GetAll());
            return await response.ToResult<List<GetAllClientInsuranceRpaConfigurationsResponse>>();
        }

        public async Task<IResult<GetClientInsuranceRpaConfigurationByIdResponse>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync($"{ClientInsuranceRpaConfigurationEndpoint.GetById(id)}");
            return await response.ToResult<GetClientInsuranceRpaConfigurationByIdResponse>();
        }

        public async Task<IResult<int>> CreateAsync(CreateClientInsuranceRpaConfigurationCommand command)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientInsuranceRpaConfigurationEndpoint.Save, command);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> UpdateAsync(UpdateClientInsuranceRpaConfigurationCommand command)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClientInsuranceRpaConfigurationEndpoint.Edit, command);
            return await response.ToResult<int>();
        }
        
        public async Task<IResult<int>> UpdateExpiryWarningAsync(UpdateRpaConfigurationExpiryWarningCommand command)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClientInsuranceRpaConfigurationEndpoint.ExpiryWarning, command);
            return await response.ToResult<int>();
        }
        

        public async Task<IResult<int>> UpdateCurrentClaimCount(UpdateRpaConfigurationDailyClaimCountCommand request)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClientInsuranceRpaConfigurationEndpoint.UpdateCurrentClaimCount(), request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientInsuranceRpaConfigurationEndpoint.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<GetClientInsuranceRpaConfigurationByCriteriaResponse>> GetSingleByCriteria(GetSingleClientInsuranceRpaConfigurationByCriteriaQuery query)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsuranceRpaConfigurationEndpoint.GetSingleByCriteria(query));
            return await response.ToResult<GetClientInsuranceRpaConfigurationByCriteriaResponse>();
        }

        public async Task<IResult<List<GetClientInsuranceRpaConfigurationByCriteriaResponse>>> GetAllByRpaInsuranceIdAsync(GetClientInsuranceRpaConfigurationByRpaInsuranceQuery query)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsuranceRpaConfigurationEndpoint.GetByRpaInsuranceId(query));
            return await response.ToResult<List<GetClientInsuranceRpaConfigurationByCriteriaResponse>>();
        }

        public async Task<IResult<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>> GetClientInsuranceRpaConfigurationsByUsernameAndUrlAsync(GetClientInsuranceRpaConfigurationsByUsernameAndUrlQuery query)
        {
            var response = await _tenantHttpClient.GetAsync(ClientInsuranceRpaConfigurationEndpoint.GetByUsernameAndUrl(query));
            return await response.ToResult<List<GetClientInsuranceRpaConfigurationsByUsernameAndUrlResponse>>();
        }

        public async Task<IResult<int>> UpdateFailureReportedAsync(UpdateClientInsuranceRpaConfigurationFailureCommand command)
        {
            var response = await _tenantHttpClient.PutAsJsonAsync(ClientInsuranceRpaConfigurationEndpoint.FailureReported, command);
            return await response.ToResult<int>();
        }

		public async Task<PaginatedResult<GetAllClientInsuranceRpaConfigurationsResponse>> GetClientInsuranceRpaConfigurationsPagedAsync(GetAllPagedClientRpaInsurangeConfigRequest request) //AA-23
		{
			var response = await _tenantHttpClient.GetAsync(ClientInsuranceRpaConfigurationEndpoint.GetAllPaged(request.PageNumber, request.PageSize));
			return await response.ToPaginatedResult<GetAllClientInsuranceRpaConfigurationsResponse>();
		}
		public async Task<IResult<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>> GetErrorOrFailedClientInsuranceRpaConfigAsync() //AA-250
		{
			var response = await _tenantHttpClient.GetAsync(ClientInsuranceRpaConfigurationEndpoint.GetErroredOrFailedCredentialConfig);
			return await response.ToResult<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>();
		}
	}
}
