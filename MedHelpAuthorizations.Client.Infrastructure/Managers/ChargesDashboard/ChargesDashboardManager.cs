using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.ChargesDashboard
{
    public class ChargesDashboardManager : IChargesDashboardManager
    {
		private readonly ITenantHttpClient _tenantHttpClient;
		public ChargesDashboardManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<IEnumerable<GetCashProjectionByDayResponse>>> GetCashProjectionByDay(GetCashProjectionByDayQuery query)
        {
            // Send an HTTP POST request to get cash projection by day
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ChargesDashboardEndPoints.GetCashProjectionByDay, query); //Updated AA-343

            // Convert the response to a result indicating the success of the operation
            var data = await response.ToResult<IEnumerable<GetCashProjectionByDayResponse>>();

            // Return the result
            return data;
        }
        public async Task<string> ExportCashProjectionByDay(ExportCashProjectionByDayQuery query)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ChargesDashboardEndPoints.ExportCashProjectionByDay, query);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IResult<IEnumerable<GetCashValueForRevenueByDayResponse>>> GetCashValueForRevenueByDay(GetCashValueForRevenueByDayQuery query)
        {
            // Send an HTTP POST request to get cash projection by day
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ChargesDashboardEndPoints.GetCashValueForRevenueByDay, query);

            // Convert the response to a result indicating the success of the operation
            var data = await response.ToResult<IEnumerable<GetCashValueForRevenueByDayResponse>>();

            // Return the result
            return data;
        }

        public async Task<string> ExportCashValueForRevenueByDay(ExportCashValueForRevenueQuery query)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.ChargesDashboardEndPoints.ExportCashValueForRevenueByDay, query);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}
