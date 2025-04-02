using MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.CustomAttributes.CustomReport;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.CustomReports
{
    public class CustomReportManager : ICustomReportManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public CustomReportManager(ITenantHttpClient tenantHttpClient) { _tenantHttpClient = tenantHttpClient; }
        public async Task<IResult<CustomReportTypeEntity>> GetFilterColumnsBasedOnReportType(CustomReportTypeEnum customReportType)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.CustomReportEndpoints.GetFilterColumnsBasedOnReportType, new GetAllFilterColumnsBasedOnReportTypeQuery() { CustomReportType = customReportType});
            return await response.ToResult<CustomReportTypeEntity>();
        }
        public async Task<IResult<UpdatedClaimReportTypePreviewModel>> GetPreviewReportForClaimReportType(CustomPreviewsReportQuery previewReportPayload)
        {
            string url = Routes.CustomReportEndpoints.GetPreviewReportForClaimReportType;
            var response = await _tenantHttpClient.PostAsJsonAsync(url, previewReportPayload);
            return await response.ToResult<UpdatedClaimReportTypePreviewModel>();
        }
        public async Task<IResult<string>> ExportPreviewReport(ExportPreviewReportQuery query)
        {
            string url = Routes.CustomReportEndpoints.ExportPreviewReportQuery;
            HttpResponseMessage response = await _tenantHttpClient.PostAsJsonAsync(url, query);
            return await response.ToResult<string>();
        }

    }
}
