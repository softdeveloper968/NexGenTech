using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Admin
{
    public interface ITenantUserService
    {
        Task AddUserToTenants(string userId, IEnumerable<int> tenantIds);
        Task AddUserToTenant(string userId, int tenantId);
        Task<List<TenantResponse>> GetAssignedTenantsAsync(string userId);
        Task RemoveAssignedTenantAsync(string userId, int tenantId);
        Task RemoveAssignedTenantsAsync(string userId, IEnumerable<int> tenantIds);
        Task UpdateAssignedTenantsAsync(string userId, IEnumerable<int> tenantIds);
    }
}
