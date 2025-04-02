using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Reports.DailyClaimReports;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.DailyMonthlyReports
{
    public class DailyClaimReportManager : IDailyClaimReportManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public DailyClaimReportManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<PaginatedResult<DailyClaimStatusReportResponse>> GetDailyClaimReportByCriteria(DailyClaimReportDetailsQuery dailyClaimReportDetails)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(DailyMonthlyReportEndpoints.GetDailyClaimReportByCriteria_NEW, dailyClaimReportDetails);
            return await response.ToPaginatedResult<DailyClaimStatusReportResponse>();
        }

    }
}
