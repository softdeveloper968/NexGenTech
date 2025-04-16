using MedHelpAuthorizations.Application.Common.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.Util;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using Tenant = MedHelpAuthorizations.Domain.IdentityEntities.Tenant;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class TenantManagementService : ITenantManagementService
    {
        private readonly ILogger<TenantManagementService> _logger;
        private readonly AdminDbContext _adminDbContext;

        public TenantManagementService(AdminDbContext adminDbContext, ILogger<TenantManagementService> logger)
        {
            _adminDbContext = adminDbContext;
            _logger = logger;
        }

        public async Task<List<TenantDto>> GetAllAsync()
        {

            var tenants = await _adminDbContext.Tenants
                .Include(x => x.DatabaseServer)
                .Select(x => new TenantDto
                {
                    Id = x.Id,
                    Identifier = x.Identifier,
                    ConnectionString = ConnectionStringBuilder.GetConnectionString(x.DatabaseServer.ServerAddress, x.DatabaseName, x.DatabaseServer.AuthenticationType, x.DatabaseServer.Username, x.DatabaseServer.Password),
                    AdminEmail = x.AdminEmail,
                    IsActive = x.IsActive,
                    TenantName = x.TenantName,
                    IsProductionTenant = x.IsProductionTenant,
                })
                .ToListAsync();

            return tenants;
        }

        public async Task<List<TenantDto>> GetAllActiveAsync() //EN-92
        {

            var tenants = await _adminDbContext.Tenants
                .Include(x => x.DatabaseServer)
                .Where(x => x.IsActive)
                .Select(x => new TenantDto
                {
                    Id = x.Id,
                    Identifier = x.Identifier,
                    ConnectionString = ConnectionStringBuilder.GetConnectionString(x.DatabaseServer.ServerAddress, x.DatabaseName, x.DatabaseServer.AuthenticationType, x.DatabaseServer.Username, x.DatabaseServer.Password),
                    AdminEmail = x.AdminEmail,
                    IsActive = x.IsActive,
                    TenantName = x.TenantName,
                })
                .ToListAsync();

            return tenants;
        }

        public async Task<bool> ExistsWithIdAsync(int id) => await _adminDbContext.Tenants.AnyAsync(x => x.Id == id);
        public async Task<bool> ExistsWithNameAsync(string name) => await _adminDbContext.Tenants.AnyAsync(x => x.TenantName == name);
        public async Task<TenantDto> GetByIdAsync(int id)
        {
            var tenant = await GetTenantInfoAsync(id);

            string connectionString = ConnectionStringBuilder.GetConnectionString(tenant.DatabaseServer.ServerAddress, tenant.DatabaseName, tenant.DatabaseServer.AuthenticationType, tenant.DatabaseServer.Username, tenant.DatabaseServer.Password);

            var retVal = new TenantDto
            {
                Id = tenant.Id,
                AdminEmail = tenant.AdminEmail,
                IsActive = tenant.IsActive,
                ConnectionString = connectionString,
                Identifier = tenant.Identifier,
                Issuer = tenant.Issuer,
                TenantName = tenant.TenantName,
                ValidUpto = tenant.ValidUpto,
            };

            return retVal;
        }
        public async Task UpdateAsync(int tenantId, int serverId, string tenantName, string tenantIdentifier, string databaseName, bool isActive, string issuer, string adminEmail, DateTime validUpto, bool isProductionTenant)
        {
            var tenant = await _adminDbContext.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId);

            if (tenant == null)
            {
                throw new NotFoundException("Tenant not found");
            }

            var dbServer = _adminDbContext.Servers.FirstOrDefault(x => x.Id == serverId);

            if (dbServer == null)
            {
                throw new NotFoundException("Invalid server id");
            }


            tenant.Identifier = tenantIdentifier;
            tenant.TenantName = tenantName;
            tenant.DatabaseName = databaseName;
            tenant.IsActive = isActive;
            tenant.AdminEmail = adminEmail;
            tenant.ValidUpto = validUpto;
            tenant.DatabaseServer = dbServer;
            tenant.IsProductionTenant = isProductionTenant;

            _adminDbContext.Update(tenant);

            await _adminDbContext.SaveChangesAsync();
        }
        public async Task<int> CreateAsync(int serverId, string tenantName, string identifier, string databaseName, bool isActive, string issuer, string adminEmail, DateTime validUpto, bool isProductionTenant)
        {
            var server = _adminDbContext.Servers.FirstOrDefault(x => x.Id == serverId);

            if (server == null)
            {
                throw new NotFoundException("Invalid server id");
            }

            var tenant = new Tenant()
            {
                Identifier = identifier,
                TenantName = tenantName,
                DatabaseServer = server,
                DatabaseName = databaseName,
                IsActive = isActive,
                AdminEmail = adminEmail,
                Issuer = issuer,
                ValidUpto = validUpto,
                IsProductionTenant = isProductionTenant //EN-546
            };

            _adminDbContext.Tenants.Add(tenant);

            await _adminDbContext.SaveChangesAsync();

            return tenant.Id;
        }
        private async Task<Tenant> GetTenantInfoAsync(int id)
        {
            var tenant = await _adminDbContext.Tenants
                                        .Include(x => x.DatabaseServer)
                                        .FirstAsync(t => t.Id == id);
            return tenant;
        }
        public async Task<TenantDto> GetByIdentifierAsync(string identifier)
        {
            Tenant tenant;
            TenantDto retVal = null;
            try
            {
                tenant = await _adminDbContext.Tenants
                                        .Include(x => x.DatabaseServer)
                                        .FirstAsync(t => t.Identifier == identifier);

                string connectionString = ConnectionStringBuilder.GetConnectionString(tenant.DatabaseServer.ServerAddress, tenant.DatabaseName, tenant.DatabaseServer.AuthenticationType, tenant.DatabaseServer.Username, tenant.DatabaseServer.Password);

                retVal = new TenantDto
                {
                    Id = tenant.Id,
                    AdminEmail = tenant.AdminEmail,
                    IsActive = tenant.IsActive,
                    ConnectionString = connectionString,
                    Identifier = tenant.Identifier,
                    Issuer = tenant.Issuer,
                    TenantName = tenant.TenantName,
                    ValidUpto = tenant.ValidUpto,
                };
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Tenant with Identifier {identifier} NOT FOUND!", ex.Message, ex.StackTrace);
            }

            return retVal;
        }
        public async Task<Tenant> GetEntityByIdAsync(int id) => await GetTenantInfoAsync(id);
        public async Task<string> GetTenantNameById(int tenantId)
        {
            return await _adminDbContext.Tenants.Where(x => x.Id == tenantId).Select(x => x.TenantName).FirstAsync();
        }
        public async Task<string> GetTenantNameByIdentifier(string tenantIdentifier)
        {
            return await _adminDbContext.Tenants.Where(x => x.Identifier == tenantIdentifier).Select(x => x.TenantName).FirstAsync();
        }
    }
}
