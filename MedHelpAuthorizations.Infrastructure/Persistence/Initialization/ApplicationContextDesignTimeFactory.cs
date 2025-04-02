using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant;
using ICurrentUserService = MedHelpAuthorizations.Application.Interfaces.Services.ICurrentUserService;
using Microsoft.AspNetCore.Http;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using MedHelpAuthorizations.Infrastructure.Shared.Services;
using System;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization

{
    public class ApplicationContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationContext>
    {
        public ApplicationContext CreateDbContext(string[] args)
        {
            ITenantInfo currentTenant = new AitTenantInfo()
            {
                //ConnectionString = @"Server=tcp:ait-db-01.database.windows.net,1433;Initial Catalog=faDbSharedTenantDb;Persist Security Info=False;User ID=aitdbadmin;Password=$l@ppyTh3Fr0922$;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; Command Timeout=360",
                ConnectionString = @"Data Source = (localdb)\\mssqllocaldb; Initial Catalog = faDbSharedTenantDb; Integrated Security = True; MultipleActiveResultSets = True",
                Id = "root",
                Identifier = "root",
                Name = "Root",
            };
            ICurrentUserService currentUserService = new CurrentUserService(new HttpContextAccessor());
            DatabaseSettingsDesignTimeFactoryOptions dbSettings = new DatabaseSettingsDesignTimeFactoryOptions()
            {
                Value = new DatabaseSettings()
                {
                    //ConnectionString = @"Server=tcp:ait-db-01.database.windows.net,1433;Initial Catalog=faDbSharedTenantDb;Persist Security Info=False;User ID=aitdbadmin;Password=$l@ppyTh3Fr0922$;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30; Command Timeout=360",
                    ConnectionString = @"Data Source = (localdb)\\mssqllocaldb; Initial Catalog = faDbSharedTenantDb; Integrated Security = True; MultipleActiveResultSets = True",
                    DBProvider = "mssql"
                }
            };


            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            optionsBuilder.UseSqlServer(currentTenant.ConnectionString,
               opts => opts.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds));

            return new ApplicationContext(currentTenant, optionsBuilder.Options, currentUserService, dbSettings);
        }
    }
}
