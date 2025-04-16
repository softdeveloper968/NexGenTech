using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class DepartmentManager : IDepartmentManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public DepartmentManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<Department>>> GetAllDepartment()
        {
            var department = await _tenantHttpClient.GetAsync(DepartmentEndpoints.GetAll());
            return await department.ToResult<List<Department>>();
        }
    }
}
