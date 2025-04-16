using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Initialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization
{
    public class IdentityDbInitializer
    {
        private readonly AdminDbContext _idContext;
        private readonly IdentityDbSeeder _dbSeeder;
        private readonly ILogger<IdentityDbInitializer> _logger;

        public IdentityDbInitializer(AdminDbContext idContext, IdentityDbSeeder dbSeeder, ILogger<IdentityDbInitializer> logger)
        {
            _idContext = idContext;           
            _dbSeeder = dbSeeder;
            _logger = logger;
            Console.WriteLine(_idContext.Database.GetDbConnection().ConnectionString);
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            if (_idContext.Database.GetMigrations().Any())
            {
                if ((await _idContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    _logger.LogInformation("Applying Migrations for Identity.");
                    await _idContext.Database.MigrateAsync(cancellationToken);
                }

                if (await _idContext.Database.CanConnectAsync(cancellationToken))
                {
                    _logger.LogInformation("Connection to Identity Database Succeeded.");

                    await _dbSeeder.SeedDatabaseAsync(_idContext, cancellationToken);
                }
            }
        }
    }
}
