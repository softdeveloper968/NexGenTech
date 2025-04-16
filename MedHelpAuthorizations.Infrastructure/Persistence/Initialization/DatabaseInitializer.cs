using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Shared.Constants.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Models.Identity;
using Microsoft.AspNetCore.Identity;
using MedHelpAuthorizations.Domain.IdentityEntities;
using Tenant = MedHelpAuthorizations.Domain.IdentityEntities.Tenant;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Configurations;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly AdminDbContext _adminDbContext;
        private readonly DatabaseSettings _dbSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TenantConfiguration _tenantSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(AdminDbContext adminDbContext, UserManager<ApplicationUser> userManager, IOptions<DatabaseSettings> dbSettings, IOptions<TenantConfiguration> tenantSettings, IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
        {
            _adminDbContext = adminDbContext;
            _dbSettings = dbSettings.Value;
            _userManager = userManager;
            _tenantSettings = tenantSettings.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
        {
            var cut = _serviceProvider.GetRequiredService<ICurrentUserService>();


            await InitializeTenantDbAsync(cancellationToken);

            var tenants = await _adminDbContext.Tenants.Include(x=>x.DatabaseServer).Where(x => x.IsActive).ToListAsync();
            foreach (var tenant in tenants)
            {
                var aitTenantInfo = new AitTenantInfo(tenant);
                await InitializeApplicationDbForTenantAsync(aitTenantInfo, cancellationToken);
            }
        }

        public async Task InitializeApplicationDbForTenantAsync(AitTenantInfo tenant, CancellationToken cancellationToken)
        {
            // First create a new scope
            using var scope = _serviceProvider.CreateScope();

            // Then set current tenant so the right connectionstring is used
            _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
                .MultiTenantContext = new MultiTenantContext<AitTenantInfo>()
                {
                    TenantInfo = tenant
                };

            // Then run the initialization in the new scope
            await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
                .InitializeAsync(cancellationToken);
        }

        private async Task InitializeTenantDbAsync(CancellationToken cancellationToken)
        {
            if (_adminDbContext.Database.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Applying Root Migrations.");
                await _adminDbContext.Database.MigrateAsync(cancellationToken);
            }

            await SeedRootTenantAsync(cancellationToken);
        }

        private async Task SeedRootTenantAsync(CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == "AITADMIN", cancellationToken);

                Server dbServer = null;

                try
                {
                    dbServer = await _adminDbContext.Servers
                        .FirstOrDefaultAsync(x => x.ServerName == _tenantSettings.ServerName && x.ServerAddress == _tenantSettings.ServerAddress, cancellationToken);

                    if (dbServer is null)
                    {
                        dbServer = new Server()
                        {
                            ServerName = _tenantSettings.ServerName,
                            ServerAddress = _tenantSettings.ServerAddress,
                            ServerType = Domain.IdentityEntities.Enums.ServerType.Database,
                            AuthenticationType = (Domain.IdentityEntities.Enums.AuthenticationType)_tenantSettings.DBAutheticationType,
                            Username = _tenantSettings.Username,
                            Password = _tenantSettings.Password,
                            CreatedBy = user?.Id ?? "Unknown",
                            LastModifiedBy = user?.Id ?? "Unknown",
                        };

                        _adminDbContext.Servers.Add(dbServer);
                        await _adminDbContext.SaveChangesAsync(cancellationToken);
                        _logger.LogInformation("Database server created successfully.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating the database server.");
                    dbServer = null; // Ensure process continues
                }

                Tenant tenant = null;

                try
                {
                    tenant = await _adminDbContext.Tenants
                        .FirstOrDefaultAsync(x => x.Identifier == _tenantSettings.Identifier && x.AdminEmail == _tenantSettings.EmailAddress, cancellationToken);

                    if (tenant is null)
                    {
                        tenant = new Tenant()
                        {
                            Identifier = _tenantSettings.Identifier,
                            TenantName = _tenantSettings.TenantName,
                            DatabaseName = _tenantSettings.DatabaseName,
                            IsActive = true,
                            ValidUpto = DateTime.UtcNow.AddYears(1),
                            AdminEmail = _tenantSettings.EmailAddress,
                            CreatedBy = user?.Id ?? "Unknown",
                            LastModifiedBy = user?.Id ?? "Unknown",
                        };

                        if (dbServer != null)
                        {
                            dbServer.Tenants.Add(tenant);
                        }
                        else
                        {
                            _adminDbContext.Tenants.Add(tenant);
                        }

                        await _adminDbContext.SaveChangesAsync(cancellationToken);
                        _logger.LogInformation("Tenant created successfully.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating the tenant.");
                }

                try
                {
                    if (user != null &&
                        (await _adminDbContext.TenantUsers.FindAsync(new object[] { 1 }, cancellationToken: cancellationToken)) is null)
                    {
                        var rootTenantUser = new TenantUser()
                        {
                            TenantId = tenant?.Id ?? 0,
                            UserId = user.Id,
                            IsActive = true
                        };

                        _adminDbContext.TenantUsers.Add(rootTenantUser);
                        await _adminDbContext.SaveChangesAsync(cancellationToken);
                        _logger.LogInformation("Root tenant user created successfully.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating the root tenant user.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Critical error occurred in SeedRootTenantAsync.");
            }
        }

    }
}
