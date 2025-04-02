using MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Initialization
{
    public class DfStagingDbInitializer
	{
        private readonly DfStagingDbContext _dbContext;
        private readonly DfStagingDbSeeder _dbSeeder;
        private readonly ILogger<DfStagingDbInitializer> _logger;
        private string tenantIdentifier;
        public DfStagingDbInitializer(DfStagingDbContext dbContext, DfStagingDbSeeder dbSeeder, ILogger<DfStagingDbInitializer> logger)
        {
            _dbContext = dbContext;
            _dbSeeder = dbSeeder;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            if (_dbContext.Database.GetMigrations().Any())
            {
                if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    _logger.LogInformation("Applying Migrations for '{tenantId}' tenant.", tenantIdentifier);
                    await _dbContext.Database.MigrateAsync(cancellationToken);
                }

                if (await _dbContext.Database.CanConnectAsync(cancellationToken))
                {
                    _logger.LogInformation("Connection to {tenantId}'s Database Succeeded.", tenantIdentifier);

                    await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
                }
            }
        }
    }
}
