using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.IdentityEntities;
using MedHelpAuthorizations.Domain.IdentityEntities.Enums;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Persistence.Initialization;
using MedHelpAuthorizations.Infrastructure.Shared.Persistence.Initialization;
using MedHelpAuthorizations.Shared.Authorization;
using MedHelpAuthorizations.Shared.Constants.Multitenancy;
using MedHelpAuthorizations.Shared.Constants.Permission;
using MedHelpAuthorizations.Shared.Constants.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;

namespace MedHelpAuthorizations.Infrastructure.Identity.Persistence.Initialization
{
    public partial class IdentityDbSeeder
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomSeederRunner _seederRunner;
        private readonly ILogger<IdentityDbSeeder> _logger;
        private string _adminUserId;
        private AdminDbContext _adminDbContext { get; set; }

        public IdentityDbSeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<IdentityDbSeeder> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _seederRunner = seederRunner;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync(AdminDbContext adminDbContext, CancellationToken cancellationToken)
        {
            try
            {
                _adminDbContext = adminDbContext;

                var connectionString = adminDbContext.Database.GetDbConnection()?.ConnectionString;
                Console.WriteLine("SeedDatabaseAsync Connection string: "+connectionString);

                // Seed Identity
                await SeedRolesAsync();
                //await SeedAdminUserAsync();
                await SeedExternalApiEnums();
				// Seed ClaimStatusCategoryMaps
				await _seederRunner.RunSeedersAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task SeedRolesAsync()
        {
            foreach (string roleName in FSHRoles.DefaultRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                    is not IdentityRole role)
                {
                    // Create the role
                    _logger.LogInformation("Seeding {role} Role", roleName);
                    role = new IdentityRole(roleName);//, $"{roleName} Role for {_currentTenant.Id} Tenant");
                    await _roleManager.CreateAsync(role);
                }

                // Assign permissions
                if (roleName == FSHRoles.Basic)
                {
                    await AssignPermissionsToRoleAsync(FSHPermissions.Basic, role);
                }
                else if (roleName == FSHRoles.Administrator)
                {
                    await AssignPermissionsToRoleAsync(FSHPermissions.Admin, role);

                    await AssignPermissionsToRoleAsync(FSHPermissions.Root, role);
                }
                else if(roleName == FSHRoles.AitSuperAdmin)
                {
                    await AssignPermissionsToRoleAsync(FSHPermissions.All, role);
                }
            }
        }

        private async Task AssignPermissionsToRoleAsync(IReadOnlyList<FSHPermission> permissions, IdentityRole role)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(c => c.Type == ApplicationClaimTypes.Permission && c.Value == permission.Name))
                {
                    _logger.LogInformation("Seeding {role} Permission '{permission}' for '{tenantId}' Tenant.", role.Name, permission.Name);
                    _adminDbContext.RoleClaims.Add(new IdentityRoleClaim<string>
                    {
                        RoleId = role.Id,
                        ClaimType = ApplicationClaimTypes.Permission,
                        ClaimValue = permission.Name,
                        //CreatedBy = "ApplicationDbSeeder"
                    });
                    await _adminDbContext.SaveChangesAsync();
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            string normalizedUserName = "AITADMIN",
                normalizedEmail = "NO-REPLY@AUTOMATEDINTEGRATIONTECHNOLOGIES.COM";

            var adminUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail && u.NormalizedUserName == normalizedUserName);
            if (adminUser is null)
            {
                adminUser = new ApplicationUser()
                {
                    FirstName = "Ait",
                    LastName = FSHRoles.Administrator,
                    Email = "no-reply@automatedintegrationtechnologies.com",
                    UserName = "aitAdmin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = "NO-REPLY@AUTOMATEDINTEGRATIONTECHNOLOGIES.COM",
                    NormalizedUserName = "AITADMIN",
                    IsActive = true,
                    PhoneNumber = "5555555555",
                    Pin = "A5555"
                };

                _logger.LogInformation("Seeding Default Admin User");
                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, UserConstants.DefaultPassword);
                await _userManager.CreateAsync(adminUser);

                _adminUserId = adminUser.Id;

                await _adminDbContext.SaveChangesAsync();
            }

            // Assign role to user
            if (!await _userManager.IsInRoleAsync(adminUser, FSHRoles.AitSuperAdmin))
            {
                _logger.LogInformation("Assigning Admin Role to Super Admin User)");
                await _userManager.AddToRoleAsync(adminUser, FSHRoles.AitSuperAdmin);
                await _adminDbContext.SaveChangesAsync();
            }

            if (!await _userManager.IsInRoleAsync(adminUser, FSHRoles.Administrator))
            {
                _logger.LogInformation("Assigning Admin Role to Admin User)");
                await _userManager.AddToRoleAsync(adminUser, FSHRoles.Administrator);
                await _adminDbContext.SaveChangesAsync();
            }

        }

		private async Task SeedExternalApiEnums()
		{
            var connectionString = _adminDbContext.Database.GetDbConnection()?.ConnectionString;
            List<ExternalApi> externalApis = new List<ExternalApi>()
			{
				new ExternalApi(){ Id = ExternalApiEnum.UHC, Code = "UhcApi", Name = "United Healthcare Api", Description = "United Healthcare Api"},
				new ExternalApi(){ Id = ExternalApiEnum.AthenaHealth, Code = "AthenaApi", Name = "Athena Health Api", Description = "Athena Health Api"},
			};
			await Task.Run(async () =>
			{
				foreach (var api in externalApis)
				{
					if (!_adminDbContext.ExternalApis.Any(a => a.Id == api.Id))
					{
						_adminDbContext.ExternalApis.Add(api);
					}
				}

				await _adminDbContext.SaveChangesAsync();
			});
			_logger.LogInformation("Seeded ExternalApis");
		}
	}
}
