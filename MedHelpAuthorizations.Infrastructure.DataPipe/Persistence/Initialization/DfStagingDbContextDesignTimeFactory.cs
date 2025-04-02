using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Http;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using MedHelpAuthorizations.Infrastructure.Shared.Services;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Identity.Services.Identity;
using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;
using Microsoft.Extensions.Configuration;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Initialization

{
    public class DataPipeContextDesignTimeFactory : IDesignTimeDbContextFactory<DfStagingDbContext>
    {
        public DfStagingDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.Development.json")
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();

            //ITenantInfo currentTenant = new AitTenantInfo()
            //{
            //    //ConnectionString = @"Server=tcp:ait-db-01.database.windows.net,1433;Initial Catalog=faDbSharedTenantDb;Persist Security Info=False;User ID=aitdbadmin;Password=$l@ppyTh3Fr0922$;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
            //    ConnectionString = @"Data Source=(localdb)\\mssqllocaldb;Initial Catalog=dataPipeDb;Integrated Security=True;MultipleActiveResultSets=True",
            //    Id = "root",
            //    Identifier = "root",
            //    Name = "Root",
            //};

            DatabaseSettingsDesignTimeFactoryOptions dbSettings = new DatabaseSettingsDesignTimeFactoryOptions()
            {
                Value = databaseSettings
            };

            ICurrentUserService currentUserService = new CurrentUserService(new HttpContextAccessor());
            ITenantClientApiKeyService tenantClientApiKeyService = new TenantClientApiKeyServiceFake(1, "initial");

            var optionsBuilder = new DbContextOptionsBuilder<DfStagingDbContext>();
            optionsBuilder.UseSqlServer(databaseSettings.DataPipeConnectionString);

            return new DfStagingDbContext(optionsBuilder.Options, dbSettings);
        }
    }
}
