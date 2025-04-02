using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Reports.ARAgingReport;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.ARAgingReports
{
    public class ARAgingReportSummaryManager : IARAgingReportSummaryManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ARAgingReportSummaryManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        /// <summary>
        /// Get A/R Aging Summary Report by criteria.
        /// </summary>
        /// <param name="claimReportDetails"> Query contains criteria defined in locations, Providers, Filter-By Date, Insurances Filters.</param>
        /// <returns></returns>
        public async Task<IResult<List<ARAgingSummaryReportResponse>>> GetARAgingSummaryClaimReportByCriteria(ARAgingSummaryClaimReportDetailsQuery claimReportDetails)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ARAgingReportEndpoints.GetARAgingSummaryClaimReportByCriteria, claimReportDetails);
            return await response.ToResult<List<ARAgingSummaryReportResponse>>();
        }

        public async Task<IResult<ARAgingDataResponse>> GetARAgingTotalsByCriteria(ARAgingDataQuery claimReportDetails)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ARAgingReportEndpoints.GetARAgingTotalsByCriteria, claimReportDetails);
            return await response.ToResult<ARAgingDataResponse>();
        }

        //EN-66
        public async Task<string> ExportReverseAnalysisReport(ARAgingDataQuery query)
        {
            //post type call to the end-point with the query parameters
            var response = await _tenantHttpClient.PostAsJsonAsync(ARAgingReportEndpoints.ExportReverseAnalysisData, query);

            //get the base64 string response and convert it to a task result
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}
