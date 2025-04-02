using MedHelpAuthorizations.Application.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Initialization
{
	public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly DfStagingDbContext _dataPipeDbContext;
        private readonly DatabaseSettings _dbSettings;
        private ITenantInfo _currentTenant;
        // private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(DfStagingDbContext _dataPipeDbContext, IOptions<DatabaseSettings> dbSettings, IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
        {
            _dataPipeDbContext = _dataPipeDbContext;
            _dbSettings = dbSettings.Value;
            _currentTenant = _currentTenant;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
        {
            var cut = _serviceProvider.GetRequiredService<ICurrentUserService>();

            await InitializeDataPipeDbAsync(cancellationToken);

            //var tenants = await _dataPipeDbContext.Tenants.ToListAsync(cancellationToken);
            //foreach (var tenant in tenants)
            //{
            //    var aitTenantInfo = new AitTenantInfo(tenant);
            //    await InitializeApplicationDbForTenantAsync(aitTenantInfo, cancellationToken);
            //}
        }

        public async Task InitializeDfStagingDbAsync(AitTenantInfo tenant, CancellationToken cancellationToken)
        {
            // First create a new scope
            using var scope = _serviceProvider.CreateScope();

            // Then set current tenant so the right connectionstring is used
            //_serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
            //    .MultiTenantContext = new MultiTenantContext<AitTenantInfo>()
            //    {
            //        TenantInfo = tenant
            //    };

            // Then run the initialization in the new scope
            await scope.ServiceProvider.GetRequiredService<DataPipeDbInitializer>()
                .InitializeAsync(cancellationToken);
        }

        public async Task InitializeDataPipeDbAsync(CancellationToken cancellationToken)
        {
            if (_dataPipeDbContext.Database.GetPendingMigrations().Any())
            {
                _logger.LogInformation("Applying Root Migrations.");
                await _dataPipeDbContext.Database.MigrateAsync(cancellationToken);
            }

            //await SeedRootTenantAsync(cancellationToken);

           // return null;
        }
    }
}
