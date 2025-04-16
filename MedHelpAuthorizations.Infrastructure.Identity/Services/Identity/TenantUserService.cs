using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class TenantUserService : ITenantUserService
    {
        private readonly AdminDbContext _adminDbContext;

        public TenantUserService(AdminDbContext adminDbContext)
        {
            _adminDbContext = adminDbContext;
        }
        public async Task AddUserToTenant(string userId, int tenantId) => await AddUserToTenants(userId, new List<int> { tenantId });

        public async Task AddUserToTenants(string userId, IEnumerable<int> tenantIds)
        {
            bool isAlreadyAssigned = await _adminDbContext.TenantUsers.AnyAsync(x => x.UserId == userId && tenantIds.Contains(x.TenantId));

            if (isAlreadyAssigned)
            {
                throw new Exception("Tenant is already assigned");
            }

            foreach (var tenantId in tenantIds)
            {
                _adminDbContext.TenantUsers.Add(new Domain.IdentityEntities.TenantUser
                {
                    TenantId = tenantId,
                    UserId = userId,
                    IsActive = true
                });
            }
            await _adminDbContext.SaveChangesAsync();
        }

        public async Task<List<TenantResponse>> GetAssignedTenantsAsync(string userId)
        {
            var allowedTenants = await _adminDbContext.TenantUsers.Where(x => x.IsActive == true && x.Tenant.IsActive && x.UserId == userId)
            .Select(x => new TenantResponse()
            {
                TenantId = x.TenantId,
                TenantIdentifier = x.Tenant.Identifier,
                Name = x.Tenant.TenantName

            }).ToListAsync();

            return allowedTenants;
        }
        public async Task RemoveAssignedTenantAsync(string userId, int tenantId) => await RemoveAssignedTenantsAsync(userId, new List<int>(tenantId));
        public async Task RemoveAssignedTenantsAsync(string userId, IEnumerable<int> tenantIds)
        {
            var tenantUsers = _adminDbContext.TenantUsers.Where(x => x.UserId == userId && tenantIds.Contains(x.TenantId)).ToList();

            _adminDbContext.TenantUsers.RemoveRange(tenantUsers);

            await _adminDbContext.SaveChangesAsync();
        }
        public async Task UpdateAssignedTenantsAsync(string userId, IEnumerable<int> tenantIds)
        {
            var alreadyAssignedTenants = await GetAssignedTenantsAsync(userId);

            var addedTenants = tenantIds.Where(x => !alreadyAssignedTenants.Any(t => t.TenantId == x)).ToList();

            var removedTenants = alreadyAssignedTenants.Where(x => !tenantIds.Any(t => t == x.TenantId) && !addedTenants.Contains(x.TenantId))
                .Select(x => x.TenantId).ToList();

            await AddUserToTenants(userId, addedTenants);

            await RemoveAssignedTenantsAsync(userId, removedTenants);
        }
    }
}
