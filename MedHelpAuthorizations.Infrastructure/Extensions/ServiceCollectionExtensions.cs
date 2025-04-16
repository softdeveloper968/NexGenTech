using Hangfire.Logging;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Reflection;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;
using System.Linq;
using MedHelpAuthorizations.Infrastructure.Services.Cryptography;
using MedHelpAuthorizations.Infrastructure.Identity.Services.Identity;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Infrastructure.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Helpers;

namespace MedHelpAuthorizations.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(ServiceCollectionExtensions));

        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        //public static IServiceCollection AddAndMigrateTenantDatabases(this IServiceCollection services, IConfiguration config)
        //{
        //    var options = services.GetOptions<TenantSettings>(nameof(TenantSettings));
        //    var defaultConnectionString = options.Defaults?.ConnectionString;
        //    var defaultDbProvider = options.Defaults?.DBProvider;
        //    if (defaultDbProvider.ToLower() == "mssql")
        //    {
        //        services.AddDbContext<ApplicationContext>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));
        //    }
        //    var tenants = options.Tenants;
        //    foreach (var tenant in tenants)
        //    {
        //        string connectionString;
        //        if (string.IsNullOrEmpty(tenant.ConnectionString))
        //        {
        //            connectionString = defaultConnectionString;
        //        }
        //        else
        //        {
        //            connectionString = tenant.ConnectionString;
        //        }
        //        using var scope = services.BuildServiceProvider().CreateScope();
        //        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        //        dbContext.Database.SetConnectionString(connectionString);
        //        if (dbContext.Database.GetMigrations().Count() > 0)
        //        {
        //            dbContext.Database.Migrate();
        //        }
        //    }
        //    return services;
        //}

        public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }
        public static IServiceCollection AddMultitenancy(this IServiceCollection services, IConfiguration config, DatabaseSettings databaseSettings)
        {
            // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
            //var databaseSettings = config.GetSection("DatabaseSettings").Get<DatabaseSettings>();
            string? identityConnectionString = databaseSettings.ConnectionString;

            if (string.IsNullOrEmpty(identityConnectionString))
            {
                throw new InvalidOperationException("DB ConnectionString is not configured.");
            }

            string? dbProvider = databaseSettings.DBProvider;
            if (string.IsNullOrEmpty(dbProvider))
            {
                throw new InvalidOperationException("DB Provider is not configured.");
            }

            _logger.Information($"Current DB Provider : {dbProvider}   ConnectionString: {identityConnectionString}");

            return services
                .AddMultiTenant<AitTenantInfo>()
                .WithStore<AitTenantStore>(ServiceLifetime.Scoped)
                   .WithDelegateStrategy(TenantIdentificationStrategy)
                    .Services
                .AddScoped<ITenantManagementService, TenantManagementService>()
                .AddScoped<ITenantMediatorService, TenantMediatorService>()
                .AddScoped<ITenantUserService, TenantUserService>()
                .AddScoped<ITenantCryptographyService, TenantCryptographyService>()
                .AddTransient<ITenantClientApiKeyService, TenantClientApiKeyService>();
              
        }
        private static Task<string?> TenantIdentificationStrategy(object context)
        {
            var httpContext = context as HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is null");
            }

            string path = httpContext.Request.Path.ToString();

            if (path.Contains("/tenant") || path.Contains("identity"))
            {
                httpContext.Request.Query.TryGetValue("t", out StringValues queryValues);

                string encryptedTenantClientId = queryValues.FirstOrDefault();

                if (string.IsNullOrEmpty(encryptedTenantClientId))
                {
                    return Task.FromResult("");
                }

                var cryptograhyService = httpContext.RequestServices.GetService<ITenantCryptographyService>();

                string tenantId = cryptograhyService.Decrypt(encryptedTenantClientId).Item1;

                return Task.FromResult(tenantId);
            }
            else
            {
                return Task.FromResult("");
            }
        }
        public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
            app.UseMultiTenant();

        private static void ConfigRoutes(Microsoft.AspNetCore.Routing.IRouteBuilder routes)
        {
            routes.MapRoute("SharedLogin", "SharedLogin", new { controller = "Home", action = "SharedLogin" });
            routes.MapRoute("Defaut", "{__tenant__=}/{controller=Home}/{action=Index}");
        }

    }
}
