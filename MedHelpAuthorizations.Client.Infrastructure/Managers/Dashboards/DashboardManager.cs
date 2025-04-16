using System.Net.Http;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.Queries.GetData;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Dashboards
{
    public class DashboardManager : IDashboardManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public DashboardManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<DashboardDataResponse>> GetDataAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.DashboardEndpoints.GetData);
            var data = await response.ToResult<DashboardDataResponse>();
            return data;
        }
    }
}