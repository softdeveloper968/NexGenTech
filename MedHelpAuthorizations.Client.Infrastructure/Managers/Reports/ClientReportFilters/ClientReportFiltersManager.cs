using MedHelpAuthorizations.Application.Features.Reports.ARAgingReport;
using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.ClientReportFilters
{
    public class ClientReportFiltersManager : IClientReportFiltersManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientReportFiltersManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<string> SaveUpdateClientReportFilters(AddEditClientReportFiltersCommand query)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ClientReportFilterEndpoints.SaveClientReportFilter, query);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> SaveCustomClientReportFilters(AddEmployeeClientUserReportFilterCommand query)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ClientReportFilterEndpoints.SaveCustomClientReportFilters, query);
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<IResult<List<GetClientReportFilterResponse>>> GetClientReportFiltersByReportId(ReportsEnum report)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ClientReportFilterEndpoints.GetClientReportFiltersByReportId, new GetClientReportFilterDetailsByReportIdQuery { ReportId = (int)report });
            return await response.ToResult<List<GetClientReportFilterResponse>>();
        }
        public async Task<IResult<ClientCustomReportFilterDetails>> GetCustomClientReportFiltersByReportId(ReportsEnum report)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ClientReportFilterEndpoints.GetCustomClientReportFilterDetailsByReportId, new GetCustomClientReportFilterDetailsByReportIdQuery { ReportId = (int)report });
            return await response.ToResult<ClientCustomReportFilterDetails>();
        }
        public async Task<IResult<List<GetClientReportFilterResponse>>> GetClientReportFiltersByReportIdAndFilterName(ReportsEnum report, string filterName)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ClientReportFilterEndpoints.GetClientReportFiltersByReportIdAndFilterName, new GetClientReportFilterDetailsByReportIdAndFilterNameQuery { ReportId = (int)report, FilterName = filterName });
            return await response.ToResult<List<GetClientReportFilterResponse>>();

        }
        public async Task<IResult<List<GetClientReportFilterResponse>>> GetClientReportFilterDetailsByClientId()
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ClientReportFilterEndpoints.GetClientReportFiltersByReportIdAndFilterName, new GetClientReportFilterDetailsByReportIdAndFilterNameQuery());
            return await response.ToResult<List<GetClientReportFilterResponse>>();

        }

        public async Task<IResult<int>> DeleteClientReportFilterById(int id)
        {
            //DeleteClientReportFilterById
            var url = Routes.ClientReportFilterEndpoints.DeleteClientReportFilterById(id);
            var response = await _tenantHttpClient.DeleteAsync(url);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteSharedClientReportFilterById(int employeeClientReportFilterId)
        {
            var url = Routes.ClientReportFilterEndpoints.DeleteSharedClientReportFilterById(employeeClientReportFilterId);
            var response = await _tenantHttpClient.DeleteAsync(url);
            return await response.ToResult<int>();
        }
    }
}
