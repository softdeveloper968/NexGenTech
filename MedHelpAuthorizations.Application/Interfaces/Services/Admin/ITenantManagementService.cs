using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Admin
{
    public interface ITenantManagementService
    {
        Task<List<TenantDto>> GetAllAsync();
        Task<bool> ExistsWithIdAsync(int id);
        Task<bool> ExistsWithNameAsync(string name);
        Task<TenantDto> GetByIdAsync(int id);
        Task<Tenant> GetEntityByIdAsync(int id);
        Task<TenantDto> GetByIdentifierAsync(string identifier);
        Task UpdateAsync(int tenantId, int serverId, string tenantName, string tenantIdentifier, string databaseName, bool isActive, string issuer, string adminEmail, DateTime validUpto, bool isProductionTenant);
        Task<int> CreateAsync(int serverId, string tenantName, string identifier, string databaseName, bool isActive, string issuer, string adminEmail, DateTime validUpto, bool isProductionTenant);
        Task<string> GetTenantNameById(int tenantId);
        Task<string> GetTenantNameByIdentifier(string tenantIdentifier);
        //Task<string> ActivateAsync(string id);
        //Task<string> DeactivateAsync(string id);
        //Task<string> UpdateSubscription(string id, DateTime extendedExpiryDate);
        Task<List<TenantDto>> GetAllActiveAsync(); //EN-91
    }
}
