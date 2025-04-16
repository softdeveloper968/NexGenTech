using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Models.MultiTenancy;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class TenantManager : ITenantManager
    {
        private readonly HttpClient _httpClient;

        public TenantManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<BasicTenantInfoResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(TenantsEndpoints.GetAll);
            return await response.ToResult<List<BasicTenantInfoResponse>>();
        }
    }
}