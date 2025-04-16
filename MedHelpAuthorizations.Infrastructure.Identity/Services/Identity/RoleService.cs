using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Shared.Constants.Permission;
using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Shared.Authorization;
using MedHelpAuthorizations.Application.Common.Exceptions;
using MedHelpAuthorizations.Shared.Constants.Multitenancy;
using MedHelpAuthorizations.Application.Identity;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Infrastructure.Shared.Extensions;
using Microsoft.Extensions.Options;
using MedHelpAuthorizations.Application.Configurations;
using MedHelpAuthorizations.Application.Extensions;


namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AdminDbContext _idContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<RoleService> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITenantInfo _currentTenant;
        private readonly IOptions<TenantConfiguration> _tenantConfiguration;

        public RoleService(
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            AdminDbContext idContext,
            IStringLocalizer<RoleService> localizer,
            ICurrentUserService currentUserService,
            ITenantInfo currentTenant,
            IOptions<TenantConfiguration> tenantConfiguration)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _currentTenant = currentTenant;
            _tenantConfiguration = tenantConfiguration;
            _idContext = idContext;
        }

        public async Task<Result<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole.Name != FSHRoles.Administrator && existingRole.Name != FSHRoles.Basic)
            {
                //TODO Check if Any Users already uses this Role
                bool roleIsNotUsed = true;
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                    {
                        roleIsNotUsed = false;
                    }
                }
                if (roleIsNotUsed)
                {
                    await _roleManager.DeleteAsync(existingRole);
                    return await Result<string>.SuccessAsync($"{_localizer["Role"]} {existingRole.Name} {_localizer["deleted."]}");
                }
                else
                {
                    return await Result<string>.FailAsync($"{_localizer["Not allowed to delete"]} {existingRole.Name} {_localizer["Role as it is being used."]}");
                }
            }
            else
            {
                return await Result<string>.FailAsync($"{_localizer["Not allowed to delete"]} {existingRole.Name} {_localizer["Role"]}.");
            }
        }

        public async Task<Result<List<RoleResponse>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
            return await Result<List<RoleResponse>>.SuccessAsync(rolesResponse);
        }

        public async Task<PaginatedResult<RoleResponse>> GetPaginatedAsync(int pageNumber, int pageSize, string search)
        {
            var roles = await _roleManager.Roles.Select(x => new RoleResponse { Id = x.Id, Name = x.Name })
                .Where(x => string.IsNullOrEmpty(search) || x.Name.Contains(search))
                .ToPaginatedListAsync(pageNumber, pageSize);
            return roles;
        }

        public async Task<Result<List<RoleNamesResponse>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleNamesResponse>>(roles);
            return await Result<List<RoleNamesResponse>>.SuccessAsync(rolesResponse);
        }

        public async Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId)
        {
            var model = new PermissionResponse();
            var allPermissions = new List<RoleClaimsResponse>();

            #region GetPermissions
            allPermissions.GetPermissions(typeof(Permissions.Authorizations), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Patients), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Users), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Roles), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Preferences), roleId);
            allPermissions.GetPermissions(typeof(Permissions.AdminUtilities), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Clients), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Notes), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Questionnaires), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Reports), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Developer), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Documents), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ClientInsurances), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ClientCptCodes), roleId);
            allPermissions.GetPermissions(typeof(Permissions.DocumentTypes), roleId);
            allPermissions.GetPermissions(typeof(Permissions.NightlyReports), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Cardholders), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ClaimStatus), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ChargeEntry), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ClientLocations), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ClientPlaceOfService), roleId);
            allPermissions.GetPermissions(typeof(Permissions.InputDocuments), roleId);
            allPermissions.GetPermissions(typeof(Permissions.IntegratedServices), roleId);
            allPermissions.GetPermissions(typeof(Permissions.InsuranceCards), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ResponsibleParty), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Providers), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Persons), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Employees), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Corporate), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ClientNote), roleId);
            allPermissions.GetPermissions(typeof(Permissions.ClientDocuments), roleId);

            //You could have your own method to refactor the below line, maybe by using Reflection and fetch directly from a class, else assume that Admin has all the roles assigned and retreive the Admin's roles here via the DB/Identity.RoleClaims table.
            allPermissions.Add(new RoleClaimsResponse { Value = "Permissions.Communication.Chat", Type = ApplicationClaimTypes.Permission });

            #endregion GetPermissions

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                model.RoleId = role.Id;
                model.RoleName = role.Name;
                var claims = await _roleManager.GetClaimsAsync(role);
                var allClaimValues = allPermissions.Select(a => a.Value).ToList();
                var roleClaimValues = claims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                foreach (var permission in allPermissions)
                {
                    if (authorizedClaims.Any(a => a == permission.Value))
                    {
                        permission.Selected = true;
                    }
                }
            }
            model.RoleClaims = allPermissions;
            return await Result<PermissionResponse>.SuccessAsync(model);
        }

        public async Task<Result<RoleResponse>> GetByIdAsync(string id)
        {
            var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            var rolesResponse = _mapper.Map<RoleResponse>(roles);
            return await Result<RoleResponse>.SuccessAsync(rolesResponse);
        }

        public async Task<Result<string>> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null) return await Result<string>.FailAsync(_localizer["Similar Role already exists."]);
                var response = await _roleManager.CreateAsync(new IdentityRole(request.Name));
                if (response.Succeeded)
                {
                    return await Result<string>.SuccessAsync(_localizer["Role Created"]);
                }
                else
                {
                    return await Result<string>.FailAsync(response.Errors.Select(e => _localizer[e.Description].Value).ToList());
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                if (existingRole.Name == FSHRoles.Administrator || existingRole.Name == FSHRoles.Basic)
                {
                    return await Result<string>.FailAsync($"{_localizer["Not allowed to modify"]} {existingRole.Name} {_localizer["Role"]}.");
                }
                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                await _roleManager.UpdateAsync(existingRole);
                return await Result<string>.SuccessAsync(_localizer["Role Updated."]);
            }
        }

        public async Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request)
        {
            var role = await _roleManager.FindByIdAsync(request.RoleId);
            _ = role ?? throw new NotFoundException(_localizer["Role Not Found"]);
            if (role.Name == FSHRoles.Administrator)
            {
                throw new ConflictException(_localizer["Not allowed to modify Permissions for this Role."]);
            }

            //if (_currentTenant.Identifier != _tenantConfiguration.Value.Identifier)
            //{
            //    // Remove Root Permissions if the Role is not created for Root Tenant.
            //    request.RoleClaims.ToList().RemoveAll(u => u.Value.StartsWith("Permissions.Root."));
            //}

            var currentClaims = await _roleManager.GetClaimsAsync(role);

            var claims = currentClaims.Where(c => request.RoleClaims.Any(p => p.Value == c.Value && !p.Selected));
            // Remove permissions that were previously selected
            foreach (var claim in claims)
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                {
                    throw new InternalServerException(_localizer["Update permissions failed."], removeResult.GetErrors(_localizer));
                }
            }

            // Add all permissions that were not previously selected
            foreach (string permission in request.RoleClaims.Where(c => c.Selected && !currentClaims.Any(p => p.Value == c.Value)).Select(c => c.Value).ToList())
            {
                if (!string.IsNullOrEmpty(permission))
                {
                    _idContext.RoleClaims.Add(new IdentityRoleClaim<string>
                    {
                        RoleId = role.Id,
                        ClaimType = ApplicationClaimTypes.Permission,
                        ClaimValue = permission,
                        //CreatedBy = _currentUserService.UserId
                    });
                    await _idContext.SaveChangesAsync();
                }
            }

            return Result<string>.Success(_localizer["Permissions Updated."]);
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _roleManager.Roles.CountAsync();
            return count;
        }
    }
}