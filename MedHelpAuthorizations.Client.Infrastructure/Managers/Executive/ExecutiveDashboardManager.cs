using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Executive
{
    internal class ExecutiveDashboardManager : IExecutiveDashboardManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ExecutiveDashboardManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<IEnumerable<ExecutiveSummary>>> GetCurrentMonthChargesAsync(GetExecutiveSummaryDataQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ExecutiveDashboardEndpoints.CurrentMonthCharges(query.ClientId));
                var data = await response.ToResult<IEnumerable<ExecutiveSummary>>();
                return data;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
