using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using self_pay_eligibility_api.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetByCriteria;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetAllPaged;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.Eligibility
{
    public class ExportDataManager : IExportDataManager
    {
        private readonly ISelfPayEligibilityHttpClient _httpClient;

        public ExportDataManager(ISelfPayEligibilityHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<GetEligibilityCheckDetailsByCriteriaResponse>>> GetDetailsByCriteriaAsync(GetEligibilityCheckDetailsByCriteriaQuery request)
        {
            var response = await _httpClient.Client.GetAsync(ExportDataEndpoint.GetDetailsDataByCriteria(request));

            return await response.ToResult<List<GetEligibilityCheckDetailsByCriteriaResponse>>();
        }
    }
}
