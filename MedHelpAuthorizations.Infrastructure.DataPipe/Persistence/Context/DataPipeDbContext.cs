using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Entities.DataPipe;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;
using MedHelpAuthorizations.Application.Features.IntegratedServices.DataPipes.Base;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Context
{
    public class DataPipeDbContext : DbContext
    {
        protected readonly ICurrentUserService _currentUserService;
        protected readonly ITenantClientApiKeyService _tenantClientApiKeyService;
        private readonly DatabaseSettings _dbSettings;
        private AitTenantInfo _currentTenant;
        public string? TenantIdentifier => _tenantClientApiKeyService.TenantIdentifier;
        public int ClientId => _tenantClientApiKeyService.ClientId ?? 0;

        public DataPipeDbContext(ITenantInfo currentTenant, DbContextOptions<DataPipeDbContext> options, ICurrentUserService currentUserService, ITenantClientApiKeyService tenantClientApiKeyService, IOptions<DatabaseSettings> dbSettings)
        {
            //_currentTenant = currentTenant;
            _tenantClientApiKeyService = tenantClientApiKeyService;
            _currentUserService = currentUserService;
            _dbSettings = dbSettings.Value;
        }

        #region DataPipe Staging Table

        public DbSet<Stg_Patient> Stg_Patients { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: We want this only for development probably... maybe better make it configurable in logger.json config?
            optionsBuilder.EnableSensitiveDataLogging();

            // If you want to see the sql queries that efcore executes:

            // Uncomment the next line to see them in the output window of visual studio
            // optionsBuilder.LogTo(m => Debug.WriteLine(m), LogLevel.Information);

            // Or uncomment the next line if you want to see them in the console
            // optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
            optionsBuilder.UseSqlServer(_dbSettings.DataPipeConnectionString, b => b.MigrationsAssembly(typeof(DataPipeDbContext).Assembly.FullName));
            //optionsBuilder.UseDatabase(_dbSettings.DBProvider!, _dbSettings.DataPipeConnectionString);
            
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserService.UserId;                       
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            foreach (var entry in ChangeTracker.Entries<IDataPipeEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.TenantIdentifier = TenantIdentifier;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            base.OnModelCreating(builder);

            builder.Entity<Stg_Patient>(entity =>
            {
                entity.ToTable(name: "Stg_Patients", SchemaNames.DataPipe);
                entity.HasQueryFilter(p => p.TenantIdentifier == TenantIdentifier);
                entity.HasQueryFilter(p => p.ClientId == ClientId);
            });
        }
    }
}
