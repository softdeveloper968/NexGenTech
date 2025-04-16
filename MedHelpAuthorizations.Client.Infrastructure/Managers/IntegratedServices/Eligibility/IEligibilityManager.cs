using MedHelpAuthorizations.Shared.Wrapper;
using self_pay_eligibility_api.Application.Features.Administration.ClientLocations.Queries.GetAll;
using self_pay_eligibility_api.Application.Features.Administration.ClientProviders.Queries.GetAll;
using self_pay_eligibility_api.Application.Features.Dashboards.Queries.GetData;
using self_pay_eligibility_api.Application.Features.Eligibility.DiscoveredEligibility.Queries.GetByCriteria;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetAllPaged;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetByCriteria;
using self_pay_eligibility_api.Application.Features.Eligibility.Payers.Queries.GetAll;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.Eligibility
{
    public interface IEligibilityManager : IManager
    {
        Task<IResult<List<GetAllPayersResponse>>> GetPayersAsync(GetAllPayersQuery request);
        Task<IResult<List<GetAllClientLocationsResponse>>> GetClientLocationsAsync(GetAllClientLocationsQuery request);
        Task<PaginatedResult<GetAllPagedDiscoveredEligibilitiesResponse>> GetAllDiscoveredEligibilitiesAsync(GetAllPagedDiscoveredEligibilitiesQuery request);
        Task<PaginatedResult<GetDiscoveredEligibilitiesByCriteriaResponse>> GetDiscoveredEligibilitiesByCriteriaAsync(GetDiscoveredEligibilitiesByCriteriaQuery request);
        Task<IResult<EligibilityDashboardDataResponse>> GetSelfPayEligibilityDashboardDataAsync(EligibilityDashboardDataQuery request);
        Task<IResult<List<GetAllClientProvidersResponse>>> GetClientProvidersAsync(GetAllClientProvidersQuery request);
        Task<PaginatedResult<GetEligibilityCheckRequestByCriteriaResponse>> GetEligibilityCheckRequestByCriteriaAsync(GetEligibilityCheckRequestByCriteriaQuery request);
    }
}
