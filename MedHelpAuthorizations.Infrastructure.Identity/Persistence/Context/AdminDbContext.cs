using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Options;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tenant = MedHelpAuthorizations.Domain.IdentityEntities.Tenant;

namespace MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context
{
    public class AdminDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly DatabaseSettings _dbSettings;
        private readonly ICurrentUserService _currentUserService;

        public AdminDbContext(DbContextOptions<AdminDbContext> options, IOptions<DatabaseSettings> dbSettings, ICurrentUserService currentUserService)
            : base(options)
        {
            _dbSettings = dbSettings.Value;
            _currentUserService = currentUserService;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<Server>(entity =>
            {
                entity.ToTable("Servers", SchemaNames.Admin);

                entity.HasIndex(x => x.ServerName).IsUnique();

                entity.HasIndex(x => x.ServerAddress);

                entity.Property(x => x.ServerName)
                .IsRequired()
                .HasMaxLength(100);


                entity.Property(x => x.ServerAddress)
                .IsRequired()
                .HasMaxLength(250);

                entity.Property(x => x.ServerType)
                .IsRequired()
                .HasColumnType("int");

                entity.Property(x => x.AuthenticationType)
                .IsRequired()
                .HasColumnType("int");

                entity.Property(x => x.Username)
                .HasMaxLength(100);

                entity.Property(x => x.Password)
                .HasMaxLength(50);

            });

            builder.Entity<Tenant>(entity =>
            {
                entity.Property(x => x.Identifier).IsRequired().HasMaxLength(20);
                entity.Property(x => x.TenantName).IsRequired().HasMaxLength(50);
                entity.Property(x => x.DatabaseName).IsRequired().HasMaxLength(50);
                entity.Property(x => x.AdminEmail).IsRequired().HasMaxLength(200);
                entity.Property(x => x.IsActive).IsRequired().HasDefaultValue(1);
                entity.Property(x => x.ValidUpto).IsRequired();


                entity.ToTable("Tenants", SchemaNames.MultiTenancy);
            });

            builder.Entity<TenantUser>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => new { x.TenantId, x.UserId }).IsUnique();
                entity.ToTable("TenantUsers", SchemaNames.MultiTenancy);
            });

			builder.Entity<ExternalApi>(entity =>
			{
				entity.Property(x => x.Code).IsRequired().HasMaxLength(16);
				entity.Property(x => x.Name).IsRequired().HasMaxLength(32);
				entity.Property(x => x.Description).IsRequired().HasMaxLength(128);
				entity.ToTable("ExternalApis", SchemaNames.Integrations);
			});

			//builder.Entity<UsedPassword>(entity =>
			//{
			//    entity.HasKey(x => x.UserId); // Assuming Id is the primary key, adjust it accordingly
			//    entity.Property(x => x.HashedPassword).IsRequired();
			//    entity.Property(x => x.CreatedOn).IsRequired();

			//    entity.ToTable("UsedPasswords", SchemaNames.Identity); // Adjust the schema name if needed
			//});

			builder.HasDefaultSchema("Identity");

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<UserLoginHistory>(entity =>
            {
                entity.ToTable("UserLoginHistory");
                entity.HasIndex(x => x.CreatedOn);
                entity.HasIndex(x => x.LoginTime);
                entity.HasIndex(x => x.LogoutTime);
                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.LastModifiedOn);
            });

            builder.Entity<UsedPassword>(entity =>
            {
                entity.ToTable("UsedPasswords");
                entity.HasIndex(x => x.CreatedOn);
                entity.HasIndex(x => x.LastModifiedOn);
                entity.HasIndex(x => x.UserId);
                entity.HasIndex(x => x.HashedPassword);
            });
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            var conn = ChangeTracker.Context.Database.GetDbConnection()?.ConnectionString;
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        if (string.IsNullOrEmpty(entry.Entity.CreatedBy))
                        {
                            entry.Entity.CreatedBy = _currentUserService.UserId;
                        }
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        #region DbSets

        public virtual DbSet<Server> Servers { get; set; }
        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<TenantUser> TenantUsers { get; set; }
        public virtual DbSet<UsedPassword> UsedPasswords { get; set; } 
        public virtual DbSet<UserLoginHistory> UserLoginHistory { get; set; }
		public virtual DbSet<ExternalApi> ExternalApis { get; set; }

		#endregion
	}
}