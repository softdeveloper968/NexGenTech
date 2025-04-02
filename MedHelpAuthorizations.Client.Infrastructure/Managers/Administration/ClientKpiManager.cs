using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetBillingKpi;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientKpiManager : IClientKpiManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public ClientKpiManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientKpisCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientKpisEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        /// <summary>
        /// Get Client Kpi data by client Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IResult<ClientKpiDto>> GetClientkpiByClientIdAsync(int clientId)
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(ClientKpisEndpoints.GetByClientId(clientId));
                return await response.ToResult<ClientKpiDto>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IResult<GetBillingKpiByClientIdResponse>> GetBillingKpiByClientIdAsync(GetBillingKpiByClientIdQuery query)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(ClientKpisEndpoints.GetBillingKpiByClientId, query);
                return await response.ToResult<GetBillingKpiByClientIdResponse>();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
