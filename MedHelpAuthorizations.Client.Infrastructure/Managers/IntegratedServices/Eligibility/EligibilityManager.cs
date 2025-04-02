using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using self_pay_eligibility_api.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using self_pay_eligibility_api.Application.Features.Eligibility.DiscoveredEligibility.Queries.GetByCriteria;
using self_pay_eligibility_api.Application.Features.Eligibility.Payers.Queries.GetAll;
using System.Collections.Generic;
using System.Threading.Tasks;
using self_pay_eligibility_api.Application.Features.Administration.ClientLocations.Queries.GetAll;
using self_pay_eligibility_api.Application.Features.Dashboards.Queries.GetData;
using self_pay_eligibility_api.Application.Features.Administration.ClientProviders.Queries.GetAll;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetByCriteria;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.Eligibility
{
    public class EligibilityManager : IEligibilityManager
    {
        private readonly ISelfPayEligibilityHttpClient _httpClient;

        public EligibilityManager(ISelfPayEligibilityHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaginatedResult<GetAllPagedDiscoveredEligibilitiesResponse>> GetAllDiscoveredEligibilitiesAsync(GetAllPagedDiscoveredEligibilitiesQuery request)
        {
            var response = await _httpClient.Client.GetAsync(DiscoveredEligibilityEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedDiscoveredEligibilitiesResponse>();
        }

        public async Task<PaginatedResult<GetDiscoveredEligibilitiesByCriteriaResponse>> GetDiscoveredEligibilitiesByCriteriaAsync(GetDiscoveredEligibilitiesByCriteriaQuery request)
        {
            var response = await _httpClient.Client.GetAsync(DiscoveredEligibilityEndpoints.GetByCriteria(request));
            return await response.ToPaginatedResult<GetDiscoveredEligibilitiesByCriteriaResponse>();
        }

        public async Task<IResult<List<GetAllClientLocationsResponse>>> GetClientLocationsAsync(GetAllClientLocationsQuery request)
        {
            var response = await _httpClient.Client.GetAsync(ClientLocationsEndpoints.GetAll());
            return await response.ToResult<List<GetAllClientLocationsResponse>>();
        }

        public async Task<IResult<List<GetAllPayersResponse>>> GetPayersAsync(GetAllPayersQuery request)
        {
            var response = await _httpClient.Client.GetAsync(PayerEndpoints.Get);
            return await response.ToResult<List<GetAllPayersResponse>>();
        }

        public async Task<IResult<EligibilityDashboardDataResponse>> GetSelfPayEligibilityDashboardDataAsync(EligibilityDashboardDataQuery request)
        {
            var response = await _httpClient.Client.GetAsync(EligibilityDashboardEndpoints.GetData(request));
            return await response.ToResult<EligibilityDashboardDataResponse>();
        }

        public async Task<IResult<List<GetAllClientProvidersResponse>>> GetClientProvidersAsync(GetAllClientProvidersQuery request)
        {
            var response = await _httpClient.Client.GetAsync(ClientProvidersEndpoints.GetAllProviders());
            return await response.ToResult<List<GetAllClientProvidersResponse>>();
        }

        public async Task<PaginatedResult<GetEligibilityCheckRequestByCriteriaResponse>> GetEligibilityCheckRequestByCriteriaAsync(GetEligibilityCheckRequestByCriteriaQuery request)
        {
            var response = await _httpClient.Client.GetAsync(EligibilityCheckRequestEndpoints.GetByCriteria(request));
            return await response.ToPaginatedResult<GetEligibilityCheckRequestByCriteriaResponse>();
        }

    }
}
