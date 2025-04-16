using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.DenialReasons
{
    public class DenialReasonsManager : IDenialReasonsManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public DenialReasonsManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<ClaimSummary>>> GetDenialsByProcedureQueryAsync(GetDenialsByProcedureQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.DenialReasonsEndPoints.GetDenialsByProcedure, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task<IResult<List<ClaimSummary>>> GetDenialsByInsuranceQueryAsync(GetDenialReasonsByInsuranceQuery criteria)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(Routes.DenialReasonsEndPoints.GetDenialsByInsurance, criteria);
                var data = await response.ToResult<List<ClaimSummary>>();
                return data;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
