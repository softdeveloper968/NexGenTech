using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllByProviderId;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetLocationDataByClientId;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientLocationsManager : IClientLocationsManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientLocationsManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientLocationsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClientLocationsEndpoints.Export);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IResult<List<GetClientLocationsByClientIdResponse>>> GetAllClientLocationsAsync()
        {
            var response = await _tenantHttpClient.GetAsync(ClientLocationsEndpoints.GetAll());
            return await response.ToResult<List<GetClientLocationsByClientIdResponse>>();
        }

        //AA-106
        public async Task<IResult<List<GetClientLocationsByProviderIdResponse>>> GetAllClientLocationsByProviderIdAsync(int providerId)
        {
            var response = await _tenantHttpClient.GetAsync(ClientLocationsEndpoints.GetAllByProviderId(providerId));
            return await response.ToResult<List<GetClientLocationsByProviderIdResponse>>();
        }

        public async Task<PaginatedResult<GetAllClientLocationsResponse>> GetAllClientLocationsPagedAsync(GetAllPagedLocationsRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientLocationsEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllClientLocationsResponse>();
        }

        public async Task<IResult<GetClientLocationByIdResponse>> GetLocaationByIdAsync(GetClientLocationByIdQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientLocationsEndpoints.GetById(request.Id));
            return await response.ToResult<GetClientLocationByIdResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientLocationCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        /// <summary>
        /// Get Location Data By Client Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IResult<List<GetClientLocationsByClientIdResponse>>> GetLocationByClientIdAsync(GetLocationDataByClientIdQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientLocationsEndpoints.GetByClientId(request.ClientId));
            return await response.ToResult<List<GetClientLocationsByClientIdResponse>>();            
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetProcedureTotalsByLocationAsync(GetProcedureTotalsByLocationQuery request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.GetProcedureTotalsByLocation, request);
            return await response.ToResult<List<ClaimSummary>>();            
        }

        public async Task<IResult<List<ClaimSummary>>> GetInsuranceTotalsByLocationAsync(GetInsuranceTotalsByLocationQuery request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.GetInsuranceTotalsByLocation, request);
            return await response.ToResult<List<ClaimSummary>>();
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetDenialReasonsByLocationsAsync(GetDenialReasonsByLocationsQuery request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.GetDenialReasonsByLocation, request);
            return await response.ToResult<List<ClaimSummary>>();
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetProcedureReimbursementByLocationAsync(GetProcedureReimbursementByLocationQuery request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.GetProcedureReimbursementByLocation, request);
            return await response.ToResult<List<ClaimSummary>>();
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetPayerReimbursementByLocationAsync(GetPayerReimbursementByLocationQuery request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.GetPayerReimbursementByLocation, request);
            return await response.ToResult<List<ClaimSummary>>();
        }
        
        public async Task<IResult<List<AverageDaysByLocation>>> GetAverageDaysToPayByLocationAsync(GetAverageDaysToPayByLocationQuery request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.GetAverageDaysToPayByLocation, request);
            return await response.ToResult<List<AverageDaysByLocation>>();
        }

        public async Task<IResult<List<ChargesTotalsByLocation>>> GetChargesByLocationAsync(GetChargesByLocationQuery request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientLocationsEndpoints.GetCharges, request);
            return await response.ToResult<List<ChargesTotalsByLocation>>();
        }
    }
}
