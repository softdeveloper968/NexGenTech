using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Shared.Wrapper;
using self_pay_eligibility_api.Application.Features.Eligibility.DiscoveredEligibility.Queries.GetByCriteria;
using self_pay_eligibility_api.Client.Infrastructure.Routes;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.Eligibility
{
    public class DiscoveredEligibilityManager : IDiscoveredEligibilityManager
    {
        private readonly HttpClient _httpClient;

        public DiscoveredEligibilityManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new System.Uri(ExternalApiConstants.SelfPayEligibilityBaseUrl);
        }

        public async Task<PaginatedResult<GetAllPagedDiscoveredEligibilitiesResponse>> GetAllAsync(GetAllPagedDiscoveredEligibilitiesQuery request)
        {
            var response = await _httpClient.GetAsync(DiscoveredEligibilityEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedDiscoveredEligibilitiesResponse>();
        }

        public async Task<PaginatedResult<GetDiscoveredEligibilitiesByCriteriaResponse>> GetByCriteriaAsync(GetDiscoveredEligibilitiesByCriteriaQuery request)
        {
            var response = await _httpClient.GetAsync(DiscoveredEligibilityEndpoints.GetByCriteria(request));
            return await response.ToPaginatedResult<GetDiscoveredEligibilitiesByCriteriaResponse>();
        }
    }
}
