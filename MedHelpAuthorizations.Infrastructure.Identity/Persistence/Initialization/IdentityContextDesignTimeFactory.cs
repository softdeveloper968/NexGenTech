using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;

namespace MedHelpAuthorizations.Infrastructure.Identity.Persistence.Initialization

{
    public class IdentityContextDesignTimeFactory : IDesignTimeDbContextFactory<AdminDbContext>
    {
        public AdminDbContext CreateDbContext(string[] args)
        {
            DatabaseSettingsDesignTimeFactoryOptions dbSettings = new DatabaseSettingsDesignTimeFactoryOptions()
            {
                Value = new DatabaseSettings()
                {
                    ConnectionString = @"Data Source=localhost;Initial Catalog=faIdentityDb;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=true",
                    //ConnectionString= @"Server=tcp:ait-db-01.database.windows.net,1433;Initial Catalog=faIdentityDb;Persist Security Info=False;User ID=aitdbadmin;Password=$l@ppyTh3Fr0922$;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
                    DBProvider = "mssql"
                }
            };

            var optionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<AdminDbContext>();
            optionsBuilder.UseSqlServer(dbSettings.Value.ConnectionString, e => {
                e.EnableRetryOnFailure();
            });
            
            return new AdminDbContext(optionsBuilder.Options, dbSettings, null);
        }
    }
}
