using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.Comparison
{
    public class ComparisonDashboardManager : IComparisonDashboardManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ComparisonDashboardManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        //getcomparisondata
        //
        public async Task<IResult<List<ProviderComparisonResponse>>> GetProviderDetails(ProviderComparisonQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ComparisonDahsboardEndpoints.GetProviderDetails, query);
                var result =  await response.ToResult<List<ProviderComparisonResponse>>();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
