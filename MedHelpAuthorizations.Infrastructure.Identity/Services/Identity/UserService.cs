using AutoMapper;
using AutoMapper.Internal;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Features.UserClients.Queries.GetByUserId;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Application.Requests.Mail;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.IdentityEntities;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Shared.Authorization;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MudBlazor;
using RazorHtmlEmails.RazorClassLib.Services;
using RazorHtmlEmails.RazorClassLib.Views.Emails.ConfirmAccount;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class UserService : IUserService
    {
        private const int UsedPasswordLimit = 10;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;
        private readonly IStringLocalizer<UserService> _localizer;
        private readonly IUserClientRepository _userClientRepository;
        private readonly AdminDbContext _adminDbContext;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ITenantUserService _tenantUserService;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITenantInfo _tenantInfo;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer; //EN-181
        private readonly IUserUsedPasswordService _userUserPasswordService;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        public UserService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ICurrentUserService currentUserService,
            RoleManager<IdentityRole> roleManager,
            IMailService mailService,
            IStringLocalizer<UserService> localizer,
            IUserClientRepository userClientRepository,
            AdminDbContext adminDbContext,
            ITenantRepositoryFactory tenantRepositoryFactory,
            ITenantUserService tenantUserService,
            ITenantInfo tenantInfo,
            IRazorViewToStringRenderer razorViewToStringRenderer,
            IUserUsedPasswordService userUsedPasswordService,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _roleManager = roleManager;
            _mailService = mailService;
            _localizer = localizer;
            _userClientRepository = userClientRepository;
            _adminDbContext = adminDbContext;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _tenantUserService = tenantUserService;
            _tenantInfo = tenantInfo;
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _userUserPasswordService = userUsedPasswordService;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<List<UserResponse>>> GetAllAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            // AutoMapper mapping of users to userResponses
            var userResponses = _mapper.Map<List<UserResponse>>(users);

            var tenants = await _adminDbContext.Tenants.ToListAsync();
            var activeTenants = tenants.Where(x => x.IsActive);

            foreach (var userResponse in userResponses)
            {
                var tenantNames = new List<string>();

                foreach (var tenant in activeTenants)
                {
                    IEmployeeRepository employeeRepository = _tenantRepositoryFactory.GetEmployeeRepository(tenant.Identifier);

                    var existingEmployee = await employeeRepository.GetEmployeeByUserId(userResponse.Id);

                    if (existingEmployee != null)
                    {
                        tenantNames.Add(tenant.TenantName);
                    }

                }

                userResponse.EmployeeLinkedTenants = tenantNames;  // Assign the list of tenant names
            }

            return await Result<List<UserResponse>>.SuccessAsync(userResponses);
        }

        public async Task<PaginatedResult<UserInfoResponse>> GetUsersPageAsync(int pageNumber, int pageSize, string search)
        {
            Expression<Func<ApplicationUser, UserInfoResponse>> expression = e => new UserInfoResponse
            {
                Id = e.Id,
                UserName = e.UserName,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                EmailConfirmed = e.EmailConfirmed,
                IsActive = e.IsActive,
                PhoneNumber = string.IsNullOrEmpty(e.PhoneNumber) ? null : long.Parse(e.PhoneNumber),
                CreatedByName = e.CreatedBy,
                CreatedOn = e.CreatedOn,
                LastModifiedByName = e.LastModifiedBy,
                LastModifiedOn = e.LastModifiedOn
            };

            PaginatedResult<UserInfoResponse> users;

            if (string.IsNullOrEmpty(search))
            {
                users = await _userManager.Users
                    .OrderByDescending(x => x.LastModifiedOn)
                    .Select(expression)
                    .ToPaginatedListAsync(pageNumber, pageSize);
            }
            else
            {
                users = await _userManager.Users
                    .Where(x => (!string.IsNullOrEmpty(x.UserName) && x.UserName.Contains(search)) ||
                                x.FirstName.Contains(search) || x.LastName.Contains(search) ||
                                (!string.IsNullOrEmpty(x.Email) && x.Email.Contains(search)))
                    .OrderByDescending(x => x.LastModifiedOn)
                    .Select(expression)
                    .ToPaginatedListAsync(pageNumber, pageSize);
            }

            var userIds = users.Data.Where(x => !string.IsNullOrEmpty(x.CreatedByName)).Select(x => x.CreatedByName).ToList();

            userIds.TryAdd(users.Data.Where(x => !string.IsNullOrEmpty(x.LastModifiedByName)).Select(x => x.LastModifiedByName).ToList());

            var names = await GetNamesAsync(userIds.ToArray());

            foreach (var user in users.Data)
            {
                if (!string.IsNullOrWhiteSpace(user.CreatedByName))
                {
                    user.CreatedByName = names[user.CreatedByName];
                }
                if (!string.IsNullOrWhiteSpace(user.LastModifiedByName))
                {
                    user.LastModifiedByName = names[user.LastModifiedByName];
                }
            }

            return users;
        }

        public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                return await Result.FailAsync($"{_localizer["Username"]} '{request.UserName}' {_localizer["is already taken."]}");
            }
            var encryptedPin = PinExtensions.EncryptPin(request.Pin);
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber.HasValue ? request.PhoneNumber.ToString() : "",
                IsActive = request.ActivateUser,
                EmailConfirmed = request.AutoConfirmEmail,
                Pin = encryptedPin  // Set the encrypted PIN here

            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, FSHRoles.Basic);
                    if (!request.AutoConfirmEmail)
                    {
                        var verificationUri = await SendVerificationEmail(user, origin);
                        var mailRequest = new MailRequest
                        {
                            From = "no-reply@automatedintegrationtechnologies.com",
                            To = user.Email,
                            Body = $@"
							{_localizer[GreetingHelpers.GetAppropriateGreetingString()]},
							Welcome to AIT Financials.
							An user account with email {user.Email} has been created for you.
							Please click the <a href='{verificationUri}'>clicking here</a> to confirm your email address and update your password.",
                            Subject = _localizer["Confirm Registration"]
                        };
                        BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                        return await Result<string>.SuccessAsync(user.Id, message: _localizer[$"User Registered. Please check your Mailbox to verify!"]);
                    }
                    await _userClientRepository.AddClientsForUserAsync(user.Id, request.ClientIds.ToHashSet());

                    if (request.CreateEmployee)
                    {
                        var getUserResult = await GetAsync(user.Id);

                        if (getUserResult.Succeeded)
                        {
                            var toggleUserStatusRequest = new ToggleUserStatusRequest
                            {
                                EmployeeNumber = request.EmployeeNumber,
                                CreateEmployee = request.CreateEmployee,
                                ActivateUser = request.ActivateUser,
                                DefaultEmployeeRoleId = request.DefaultEmployeeRoleId,
                                TenantIdentifiers = request.TenantIdentifiers,
                                UserId = user.Id,
                                IsExistingEmployee = getUserResult.Data.IsExistingEmployee
                            };

                            await ToggleUserStatusAsync(toggleUserStatusRequest);
                        }
                    }

                    return await Result<string>.SuccessAsync(user.Id, message: _localizer[$"User Registered"]);
                }
                else
                {
                    return await Result.FailAsync(result.Errors.Select(a => a.Description).ToList());
                }
            }
            else
            {
                return await Result.FailAsync($"{_localizer["Email"]} {request.Email} {_localizer["is already registered."]}");
            }
        }
        public async Task<IResult> CreateUserAsync(CreateUserRequest request, string origin)
        {
            try
            {
                var userHelper = new UserHelper();
                var pin = userHelper.GeneratePin(request.FirstName, request.PhoneNumber);
                var encryptedPin = PinExtensions.EncryptPin(pin);
                var user = new ApplicationUser
                {
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.UserName,
                    PhoneNumber = request.PhoneNumber.HasValue ? request.PhoneNumber.ToString() : "",
                    IsActive = request.IsActive,
                    EmailConfirmed = request.AutoConfirmEmail,
                    Pin = encryptedPin,
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {
                    // Add the user's password to UsedPassword table
                    var userData = await _userManager.FindByEmailAsync(request.Email);
                    if (userData != null)
                    {
                        await _userUserPasswordService.AddUsedPasswordAsync(userData.Id, userData.PasswordHash);
                    }

                    await _userManager.AddToRolesAsync(user, request.Roles);

                    await _tenantUserService.AddUserToTenants(user.Id, request.Tenants);

                    foreach (var tenantClient in request.TenantClients)
                    {
                        var clientRepo = _tenantRepositoryFactory.Get<IUserClientRepository>(tenantClient.Key);
                        await clientRepo.AddClientsForUserAsync(user.Id, tenantClient.Value);
                    }

                    if (!request.AutoConfirmEmail)
                    {
                        var verificationUri = await SendVerificationEmail(user, origin);
                        var greeting = GreetingHelpers.GetAppropriateGreetingString();
                        var confirmAccountModel = new ConfirmAccountEmailViewModel()
                        {
                            Email = request.Email,
                            ConfirmEmailUrl = verificationUri,
                            FirstName = request.FirstName,
                            Greeting = greeting
                        };
                        var path = "/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml";
                        string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, confirmAccountModel);

                        var mailRequest = new MailRequest
                        {
                            From = "no-reply@automatedintegrationtechnologies.com",
                            To = user.Email,
                            Body = body,
                            Subject = _localizer["Confirm Registration"]
                        };

                        BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                        return await Result<string>.SuccessAsync(user.Id, message: _localizer[$"User Registered. Verfication mail is sent to user email."]);
                    }
                    return await Result<string>.SuccessAsync(user.Id, message: _localizer[$"User Registered Successfully"]);
                }
                else
                {
                    return await Result.FailAsync("Failed to create user");
                }
            }
            catch (Exception ex)
            {
                return await Result.FailAsync("Failed to create user");
            }
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = $"confirmationemail";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            try
            {
                var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
                var result = _mapper.Map<UserResponse>(user);
                var TenantIdentifiers = new List<string>();
                // Retrieve assigned tenants for the user
                var assignedTenants = await GetAssignedTenantsByUserIdAsync(userId);
                if (assignedTenants.Succeeded && assignedTenants.Data.Any())
                {
                    // Extract tenant identifiers from assigned tenants
                    var tenantIdentifiers = assignedTenants.Data.Select(t => t.TenantIdentifier).ToList();

                    // Process based on the request to create or delete an employee
                    foreach (var tenant in tenantIdentifiers)
                    {
                        // Get the repository for the specific tenant context
                        IEmployeeRepository employeeRepository = _tenantRepositoryFactory.GetEmployeeRepository(tenant);

                        // Get the existing employee for the user
                        var existingEmployee = await employeeRepository.GetEmployeeByUserId(userId);
                        if (existingEmployee != null)
                        {
                            TenantIdentifiers.Add(tenant);
                        }
                    }
                }
                result.TenantIdentifiers = TenantIdentifiers.AsEnumerable();
                if (result.TenantIdentifiers.Any())
                    result.IsExistingEmployee = true;
                return await Result<UserResponse>.SuccessAsync(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task<IResult<UserMasterResponse>> GetMasterAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || (await _userManager.FindByIdAsync(userId) == null))
                {
                    throw new Exception("UserId not found");
                }

                var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
                ;
                var result = _mapper.Map<UserMasterResponse>(user);
                result.Pin = PinExtensions.DecryptPin(user.Pin);
                result.Roles = (await GetRolesAsync(userId)).Data.UserRoles;
                result.Tenants = (await GetAssignedTenantsAsync(userId)).Data;
                result.TenantClients = (await GetAssginedTenantsClientsAsync(userId)).Data;

                return Result<UserMasterResponse>.Success(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<string> GetNameAsync(string userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId)
                .Select(x => $"{x.LastName},{x.FirstName}")
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<Dictionary<string, string>> GetNamesAsync(string[] userIds)
        {
            var user = await _userManager.Users.Where(u => userIds.Contains(u.Id))
                .Select(x => new { x.FirstName, x.LastName, x.Id })
                .ToListAsync();
            return user.ToDictionary(x => x.Id, y => $"{y.LastName},{y.FirstName}");
        }

        /// <summary>
        /// Toggles the user status and performs actions based on the request.
        /// </summary>
        /// <param name="request">The request containing information for toggling user status.</param>
        /// <returns>A task representing the result of the operation.</returns>
        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            try
            {
                // Retrieve the user by user ID
                var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();

                if (user != null)
                {
                    // Check if the user is an administrator
                    var isAdmin = await _userManager.IsInRoleAsync(user, FSHRoles.Administrator);
                    if (isAdmin)
                    {
                        return await Result.FailAsync(_localizer["Administrators Profile's Status cannot be toggled"]);
                    }

                    // Process based on the user type and request parameters
                    if (user != null)
                    {
                        if (request.IsExistingEmployee)
                        {
                            // Retrieve assigned tenants for the user
                            var assignedTenants = await GetAssignedTenantsByUserIdAsync(request.UserId);

                            if (assignedTenants.Succeeded && assignedTenants.Data.Any())
                            {
                                var tenantsToAssign = request.TenantIdentifiers.Where(t => !assignedTenants.Data.ToList().Select(tenant => tenant.TenantIdentifier).Contains(t));
                                var tenantsToRemove = assignedTenants.Data.ToList().Where(t => !request.TenantIdentifiers.Contains(t.TenantIdentifier)).Select(tenant => tenant.TenantIdentifier);

                                // Assign employees to tenants
                                if (tenantsToAssign.Any())
                                {

                                    ///ToDo: Comment
                                    foreach (var assignedTenantId in tenantsToAssign.ToList())
                                    {
                                        var newEmployee = new Employee()
                                        {
                                            UserId = request.UserId,
                                            EmployeeNumber = request.EmployeeNumber,
                                            DefaultEmployeeRoleId = request.DefaultEmployeeRoleId
                                        };

                                        await AddEmployeeToTenant(assignedTenantId, newEmployee);

                                    }
                                }

                                // Remove employees from existing tenants
                                if (tenantsToRemove.Any())
                                {
                                    await RemoveEmployeeFromTenant(tenantsToRemove.ToList(), request.UserId);
                                }
                            }
                        }
                        else
                        {
                            // Process non-existing employees
                            if (request.CreateEmployee && request.TenantIdentifiers.Any())
                            {
                                foreach (var assignedTenantId in request.TenantIdentifiers.ToList())
                                {

                                    var newEmployee = new Employee()
                                    {
                                        UserId = request.UserId,
                                        EmployeeNumber = request.EmployeeNumber,
                                        DefaultEmployeeRoleId = request.DefaultEmployeeRoleId
                                    };
                                    await AddEmployeeToTenant(assignedTenantId, newEmployee);

                                }

                            }
                        }
                    }

                    // Update the user's status
                    user.IsActive = request.ActivateUser;
                    var identityResult = await _userManager.UpdateAsync(user);
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                throw; // Consider logging the exception or returning a failure result
            }

            return await Result.SuccessAsync();
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var viewModel = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId);
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRoleModel
                {
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var result = new UserRolesResponse { UserRoles = viewModel };
            return await Result<UserRolesResponse>.SuccessAsync(result);
        }

        public async Task<IResult<UserAllRolesResponse>> GetUserRolesAsync(string userId)
        {
            try
            {
                var roles = await _roleManager.Roles.ToListAsync();
                var rolesResponse = _mapper.Map<List<RoleNamesResponse>>(roles);

                List<string> selectedRoles = new List<string>();

                var user = await _userManager.FindByIdAsync(userId);

                foreach (var role in rolesResponse)
                {
                    if (await _userManager.IsInRoleAsync(user, role.NormalizedName))
                        selectedRoles.Add(role.NormalizedName);
                }

                return await Result<UserAllRolesResponse>.SuccessAsync(new UserAllRolesResponse() { Roles = rolesResponse, SelectedRoles = selectedRoles });
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IResult> UpdateRolesAsync(string userId, UpdateUserRolesRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user.Email == "no-reply@automatedintegrationtechnologies.com")
                return await Result.FailAsync(_localizer["Not Allowed."]);
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, request.UserRoles);
            return await Result.SuccessAsync(_localizer["Roles Updated"]);
        }

        public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                // Generate change password link
                // Send the changePasswordLink to the user or redirect them to the change password page
                return await Result<string>.SuccessAsync(user.Id, message: $"{_localizer["Account Confirmed for"]} {user.Email}. {_localizer["You can now use the /api/identity/token endpoint to generate JWT."]}");

            }
            else
            {
                throw new ApiException($"{_localizer["An error occurred while confirming"]} {user.Email}.");
            }
        }

        //AA-103
        public async Task<string> GenerateChangePasswordLink(ApplicationUser user, string origin)
        {
            // Generate a code for changing the password
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // Construct change password link
            var route = "account/reset-password";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var changePasswordLink = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);

            return changePasswordLink;
        }

        public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var verificationUri = await SendVerificationEmail(user, origin);

            var greeting = GreetingHelpers.GetAppropriateGreetingString();
            var confirmAccountModel = new ConfirmAccountEmailViewModel()
            {
                Email = request.Email,
                FirstName = user.FirstName,
                Greeting = greeting,
                ConfirmEmailUrl = verificationUri,
            };
            var path = "/Views/Emails/ConfirmAccount/ForgotPasswordEmail.cshtml";
            string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, confirmAccountModel);

            var mailRequest = new MailRequest()
            {
                Body = body ?? string.Empty,
                Subject = _localizer["Forgot Password"],
                To = request.Email
            };
            BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));

            return await Result.SuccessAsync(_localizer["Password Reset Mail has been sent to your authorized EmailId."]);
            /*	var mailRequest = new MailRequest()
                {
                    Body = $@"
                                {_localizer[GreetingHelpers.GetAppropriateGreetingString()]},
                                Welcome to AIT Financials.
                                An user account with email {user.Email} has been created for you.
                                Please click the <a href='{HtmlEncoder.Default.Encode(verificationUri)}'>{_localizer["clicking here"]}</a> to confirm your email address and update your password.",
                    Subject = _localizer["Reset Password"],
                    To = request.Email
                };
                BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                return await Result.SuccessAsync(_localizer["Password Reset Mail has been sent to your authorized EmailId."]);*/

        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            //var user = await GetUserAndPreviousPasswordsAsync(null, request.Email);
            var userData = await _userManager.FindByEmailAsync(request.Email);

            if (userData == null)
                return Result.Fail("User not found");

            var validation = await ValidateNewPasswordAsync(userData.Id, request.Password);

            if (!validation)
            {
                return Result.Fail("Cannot use same password");
            }

            var result = await _userManager.ResetPasswordAsync(userData, request.Token, request.Password);
            if (result.Succeeded)
            {
                //EN-178
                var hasPassword = _passwordHasher.HashPassword(userData, request.Password);
                await _userUserPasswordService.RecordUserPasswordAsync(userData.Id, hasPassword);
                var greeting = GreetingHelpers.GetAppropriateGreetingString();
                var confirmAccountModel = new ConfirmAccountEmailViewModel()
                {
                    Email = request.Email,
                    FirstName = userData.FirstName,
                    Greeting = greeting
                };
                var path = "/Views/Emails/ConfirmAccount/ResetPasswordEmail.cshtml";
                string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, confirmAccountModel);

                var mailRequest = new MailRequest()
                {
                    Body = body ?? string.Empty,
                    Subject = _localizer["Password Reset Successfully"],
                    To = request.Email
                };
                BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));

                return await Result.SuccessAsync(_localizer["Password Reset Successful!"]);
            }
            else
            {
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }
        }
        public async Task<IResult> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            var userData = await _userManager.FindByIdAsync(request.UserId);

            if (userData == null)
                return Result.Fail("User not found");

            var validation = await ValidateNewPasswordAsync(userData.Id, request.Password);

            if (!validation)
            {
                return Result.Fail("Cannot use same password");
            }

            await _userManager.RemovePasswordAsync(userData);
            var result = await _userManager.AddPasswordAsync(userData, request.Password);
            if (result.Succeeded)
            {
                var hasPassword = _passwordHasher.HashPassword(userData, request.Password);
                await _userUserPasswordService.RecordUserPasswordAsync(userData.Id, hasPassword);

                return await Result.SuccessAsync(_localizer["Password Changes Successfully"]);
            }
            else
            {
                return await Result.FailAsync(_localizer["An Error has occured!"]);
            }
        }
        public async Task<int> GetCountAsync()
        {
            var count = await _userManager.Users.CountAsync();
            return count;
        }

        public List<UserResponse> GetAllUserForPermission(string permission)
        {
            var roleids = _adminDbContext.RoleClaims.Where(x => x.ClaimValue == permission).Select(x => x.RoleId)?.ToList();

            var users = _adminDbContext.UserRoles
                .Where(x => roleids.Contains(x.RoleId))
                .Select(y => _adminDbContext.Users.FirstOrDefault(z => z.Id == y.UserId))
                .ToList();
            var result = _mapper.Map<List<UserResponse>>(users);
            return result;
        }

        public async Task<IResult<List<UserResponse>>> GetUserForPermissionAsync(string permission)
        {
            //var roleids = _blazorHeroContext.RoleClaims.Where(x => x.ClaimValue == permission).Select(x => x.RoleId)?.ToList();
            var roleids = await _adminDbContext.RoleClaims.Where(x => x.ClaimValue == permission).Select(x => x.RoleId)?.ToListAsync();

            var users = await _adminDbContext.UserRoles
                .Where(x => roleids.Contains(x.RoleId))
                .Select(y => _adminDbContext.Users.FirstOrDefault(z => z.Id == y.UserId))
                .ToListAsync();
            var clientusers = await _userClientRepository.GetUsersForClientAsync(_currentUserService.ClientId);
            users = users.Where(x => clientusers.Contains(x.Id)).ToList();
            var result = _mapper.Map<List<UserResponse>>(users);
            return await Result<List<UserResponse>>.SuccessAsync(result);
        }

        public async Task<List<ClientUserResponse>> GetAllClientUserForApplicationReport(string permission, Domain.Entities.Enums.ApplicationReportEnum ApplicationReportId)
        {
            var roleids = _adminDbContext.RoleClaims.Where(x => x.ClaimValue == permission).Select(x => x.RoleId)?.ToList();
            var users = await _adminDbContext.UserRoles
               .Where(x => roleids.Contains(x.RoleId))
               .Select(y => _adminDbContext.Users.FirstOrDefault(z => z.Id == y.UserId))
               .ToListAsync();
            var clientusers = await _userClientRepository.GetUsersForClientAsync(_currentUserService.ClientId);
            users = users.Where(x => clientusers.Contains(x.Id)).ToList();

            //var foo = await (from uc in _dbContext.UserClients
            //                   join u in _idContext.Users on uc.UserId equals u.Id
            //                   join ur in _idContext.UserRoles on uc.UserId equals ur.UserId
            //                   join cua in _dbContext.ClientUserApplicationReports on uc.Id equals cua.UserClientId
            //                   join ac in _dbContext.ApplicationReports on cua.ApplicationReportId equals ac.Id
            //                   where ac.Id == ApplicationReportId
            //                   && roleids.Contains(ur.RoleId) && ac.ApplicationFeatureId == Domain.Entities.Enums.ApplicationFeatureEnum.ClaimStatus
            //                   select new ClientUserResponse
            //                   {
            //                       ClientId = uc.ClientId,
            //                       ClientCode = uc.Client.ClientCode,
            //                       Email = u.Email,
            //                       IsActive = u.IsActive,
            //                       EmailConfirmed = u.EmailConfirmed,
            //                       ClientUserId = uc.Id
            //                   }).ToListAsync();
            return null;

            //throw new NotImplementedException();
        }

        public async Task<IResult<IEnumerable<TenantResponse>>> GetAssignedTenantsAsync(string userId)
        {
            try
            {
                var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(userId) || user == null)
                {
                    throw new Exception("UserId not found");
                }

                var allowedTenants = (await _tenantUserService.GetAssignedTenantsAsync(userId)).ToList();

                return await Result<List<TenantResponse>>.SuccessAsync(allowedTenants);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<IResult<IEnumerable<TenantResponse>>> GetAssignedTenantsAsync()
        {
            if (_currentUserService.UserId == null)
            {
                throw new Exception("UserId not found");
            }

            var allowedTenants = _adminDbContext.TenantUsers.Where(x => x.IsActive == true && x.UserId == _currentUserService.UserId)
            .Select(x => new TenantResponse()
            {
                TenantIdentifier = x.Tenant.Identifier
            }).ToList();

            foreach (var tenant in allowedTenants)
            {
                tenant.Name = _adminDbContext.Tenants.First(x => x.Identifier == tenant.TenantIdentifier).TenantName;
            }

            return await Result<List<TenantResponse>>.SuccessAsync(allowedTenants);
        }


        /// <summary>
        /// Get assigned tenants by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>List of Tenants associated with the user.</returns>
        public async Task<IResult<IEnumerable<TenantResponse>>> GetAssignedTenantsByUserIdAsync(string userId) //AA-233
        {
            try
            {
                var user = await _adminDbContext.Users.Where(x => x.IsActive == true && x.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception("UserId not found");
                }
                var allowedTenants = _adminDbContext.TenantUsers
                                        .Include(t => t.Tenant)
                                        .Where(x => x.IsActive && x.UserId == userId && x.Tenant.IsActive)
                                                                .Select(x => new TenantResponse()
                                                                {
                                                                    TenantIdentifier = x.Tenant.Identifier,
                                                                    Name = x.Tenant.TenantName,
                                                                    TenantId = x.Tenant.Id
                                                                }).ToList();

                return await Result<List<TenantResponse>>.SuccessAsync(allowedTenants);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<IResult<IEnumerable<ClientResponse>>> GetAssginedClientsAsync(string tenantIdentifier)
        {
            IUserClientRepository userClientRepository = _tenantRepositoryFactory.GetUserClientRepository(tenantIdentifier);

            var allowedClients = (await userClientRepository.GetClientsForUser(_currentUserService.UserId, true))
                .Select(x => new ClientResponse()
                {
                    ClientCode = x.ClientCode,
                    Name = x.Name
                }).ToList();

            return await Result<List<ClientResponse>>.SuccessAsync(allowedClients);
        }

        /// <summary>
        /// Add TenantUser for selected Tenant for an User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<bool> AddTenantUsersAsync(string userId, ICollection<string> tenantIdentifiers) //AA-206
        {
            foreach (var identifier in tenantIdentifiers)
            {
                var tenant = await _adminDbContext.Tenants.Where(t => t.Identifier == identifier).FirstOrDefaultAsync();
                await _adminDbContext.TenantUsers
                    .AddAsync(new TenantUser() { UserId = userId, TenantId = tenant.Id });
            }
            await _adminDbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Add Employee to each tenant db
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="newEmployee"></param>
        /// <returns></returns>
        public async Task<IResult> AddEmployeeToTenant(string tenantId, Employee newEmployee) //AA-206
        {
            try
            {
                IEmployeeRepository employeeRepository = _tenantRepositoryFactory.GetEmployeeRepository(tenantId);

                await employeeRepository.AddEmployeeForTenantAsync(newEmployee);

                return await Result.SuccessAsync(_localizer["Employee Creation Successful!"]);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Removes an employee from multiple tenants.
        /// </summary>
        /// <param name="tenantId">The collection of tenant IDs.</param>
        /// <param name="userId">The user ID of the employee to be removed.</param>
        /// <returns>A task representing the result of the operation.</returns>
        public async Task<IResult> RemoveEmployeeFromTenant(ICollection<string> tenantId, string userId) //AA-233
        {
            try
            {
                foreach (var tenant in tenantId)
                {
                    // Get the employee repository specific to the current tenant
                    IEmployeeRepository employeeRepository = _tenantRepositoryFactory.GetEmployeeRepository(tenant);

                    // Retrieve the existing employee using the provided user ID
                    var existingEmployee = await employeeRepository.GetEmployeeByUserId(userId);

                    if (existingEmployee != null)
                    {
                        // Delete associated EmployeeClients if any
                        if (existingEmployee.EmployeeClients.Any())
                        {
                            foreach (var employeeClient in existingEmployee.EmployeeClients)
                            {
                                await employeeRepository.DeleteEmployeeClientAsync(employeeClient.Id);
                            }
                        }

                        // Delete the existing employee
                        await employeeRepository.DeleteAsync(existingEmployee);
                    }
                }

                return await Result.SuccessAsync(_localizer["Employee Removal Successful!"]);
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                throw; // Consider logging the exception or returning a failure result
            }
        }

        public async Task<IResult<IEnumerable<BasicTenantClientResponse>>> GetAssginedTenantsClientsAsync(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || (await _userManager.FindByIdAsync(userId) == null))
                {
                    throw new Exception("UserId not found");
                }

                var tenants = _adminDbContext.TenantUsers
                    .Include(x => x.Tenant)
                    .Where(x => x.IsActive && x.UserId == userId 
                            && x.Tenant != null && x.Tenant.IsActive)
                .Select(x => new
                {
                    TenantId = x.TenantId,
                    TenantIdentifier = x.Tenant.Identifier,
                    TenantName = x.Tenant.TenantName
                }).ToList();

                List<BasicTenantClientResponse> basicTenantClientResponses = new List<BasicTenantClientResponse>();

                foreach (var tenant in tenants)
                {
                    IUserClientRepository userClientRepository = _tenantRepositoryFactory.GetUserClientRepository(tenant.TenantIdentifier);
                    IEmployeeRepository employeeRepository = _tenantRepositoryFactory.GetEmployeeRepository(tenant.TenantIdentifier);
                    IEmployeeClientRepository employeeClientRepository = _tenantRepositoryFactory.Get<IEmployeeClientRepository>(tenant.TenantIdentifier);

                    List<BasicClientReponse> allowedClients = (await userClientRepository.GetClientsForUser(userId))
                                            .Select(x => new BasicClientReponse()
                                            {
                                                ClientId = x.Id,
                                                ClientName = x.Name
                                            }).ToList();

                    var employee = await employeeRepository.GetEmployeeByUserId(userId) ?? new Employee();
                    var employeeClients = (await employeeClientRepository.GetByEmployeeId(employee.Id)).Select(x => new BasicClientReponse()
                    {
                        ClientId = x.ClientId,
                        ClientName = x.Client.Name
                    }).ToList() ?? new List<BasicClientReponse>();

                    basicTenantClientResponses.Add(new BasicTenantClientResponse
                    {
                        TenantId = tenant.TenantId,
                        TenantName = tenant.TenantName,
                        Clients = allowedClients,
                        ClientEmployees = employeeClients,
                    });
                }

                return Result<IEnumerable<BasicTenantClientResponse>>.Success(basicTenantClientResponses);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<IResult> UpdateTenantClients(string userId, UpdateUserTenantClientRequest request)
        {
            if (string.IsNullOrEmpty(userId) || (await _userManager.FindByIdAsync(userId) == null))
            {
                throw new Exception("UserId not found");
            }
            try
            {
                var alreadyAssignedTenants = await _tenantUserService.GetAssignedTenantsAsync(userId);

                var addedTenants = request.TenantClients.Keys.Where(x => !alreadyAssignedTenants.Any(t => t.TenantId == x)).ToList();

                var removedTenants = alreadyAssignedTenants.Where(x => !request.TenantClients.Keys.Any(t => t == x.TenantId) && !addedTenants.Contains(x.TenantId))
                    .Select(x => x.TenantId).ToList();

                foreach (var tenantId in removedTenants)
                {
                    var clientRepo = _tenantRepositoryFactory.Get<Application.Interfaces.Repositories.IUserClientRepository>(tenantId);
                    await clientRepo.UpdateClientsForUserAsync(userId, new List<int>());
                }

                await _tenantUserService.UpdateAssignedTenantsAsync(userId, request.TenantClients.Keys);

                foreach (var tenantClient in request.TenantClients)
                {
                    var clientRepo = _tenantRepositoryFactory.Get<Application.Interfaces.Repositories.IUserClientRepository>(tenantClient.Key);
                    await clientRepo.UpdateClientsForUserAsync(userId, tenantClient.Value);
                }

                return Result.Success("User Tenant and Client updated successfully.");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync(new List<string>() { "Failed to update tenant/client. Method: UserService.UpdateTenantClients(string, UpdateUserTenantClientRequest)", ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }
        public async Task<IResult<Dictionary<int, List<int>>>> GetEmployeeTenantClients(string userId)
        {
            if (string.IsNullOrEmpty(userId) || (await _userManager.FindByIdAsync(userId) == null))
            {
                throw new Exception("UserId not found");
            }

            var tenants = _adminDbContext.TenantUsers.Where(x => x.IsActive == true && x.UserId == userId)
                            .Select(x => new
                            {
                                TenantId = x.TenantId,
                                TenantIdentifier = x.Tenant.Identifier,
                                TenantName = x.Tenant.TenantName
                            }).ToList();

            Dictionary<int, List<int>> response = new Dictionary<int, List<int>>();

            foreach (var tenant in tenants)
            {

                IUserClientRepository userClientRepository = _tenantRepositoryFactory.GetUserClientRepository(tenant.TenantIdentifier);
                IEmployeeRepository employeeRepository = _tenantRepositoryFactory.GetEmployeeRepository(tenant.TenantIdentifier);
                IEmployeeClientRepository employeeClientRepository = _tenantRepositoryFactory.Get<IEmployeeClientRepository>(tenant.TenantIdentifier);

                var employee = await employeeRepository.GetEmployeeByUserId(userId);

                List<int> employeeClients = new List<int>();

                if (employee != null)
                {
                    employeeClients = (await employeeClientRepository.GetByEmployeeId(employee.Id)).Select(x => x.ClientId).ToList();
                }

                response.Add(tenant.TenantId, employeeClients ?? new List<int>());

            }

            return Result<Dictionary<int, List<int>>>.Success(response);
        }
        public async Task<IResult> UpdateEmployeeTenantClients(string userId, Dictionary<int, List<int>> request)
        {
            if (string.IsNullOrEmpty(userId) || (await _userManager.FindByIdAsync(userId) == null))
            {
                throw new Exception("UserId not found");
            }
            try
            {
                foreach (var tenant in request.Keys)
                {
                    var employeeRepo = _tenantRepositoryFactory.Get<IEmployeeRepository>(tenant);

                    var employeeClientRepo = _tenantRepositoryFactory.Get<IEmployeeClientRepository>(tenant);

                    var employee = await employeeRepo.GetEmployeeByUserId(userId);

                    if (employee == null)
                    {
                        var isCreated = await employeeRepo.AddEmployeeForTenantAsync(new Employee()
                        {
                            UserId = userId,
                        });

                        employee = await employeeRepo.GetEmployeeByUserId(userId);
                    }

                    var existingEmployeeClients = await employeeClientRepo.GetByEmployeeId(employee.Id);

                    var addEmployeeClients = request[tenant].Where(x => !existingEmployeeClients.Any(t => t.ClientId == x)).ToList();

                    var removedEmployeeClients = existingEmployeeClients
                                                    .Where(x => !request[tenant].Any(t => t == x.ClientId) && !addEmployeeClients.Contains(x.ClientId))
                                                    .ToList();

                    employeeClientRepo.RemoveRange(removedEmployeeClients);

                    employeeClientRepo.AddRange(addEmployeeClients.Select(x => new EmployeeClient()
                    {
                        ClientId = x,
                        EmployeeId = employee.Id,
                        AssignedAverageDailyClaimCount = 0,
                        ExpectedMonthlyCashCollections = 0,
                        ReceiveReport = false,
                    }));


                    await employeeClientRepo.Commit(CancellationToken.None);

                }
                return Result.Success("Client's Employees updated successfully");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync(new List<string>() { "Failed to update tenant's client employee. Method: UserService.UpdateEmployeeTenantClients(string userId, Dictionary<int, List<int>> request)", ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }
        public async Task<IResult> UpdateClients(string userId, int tenantId, List<int> clientIds)
        {
            try
            {
                var clientRepo = _tenantRepositoryFactory.Get<Application.Interfaces.Repositories.IUserClientRepository>(tenantId);
                await clientRepo.UpdateClientsForUserAsync(userId, clientIds);
                return Result.Success("User Clients updated successfully.");
            }
            catch (Exception ex)
            {
                return await Result.FailAsync(new List<string>() { "Failed to update client. Method: UserService.UpdateTenantClients(string, UpdateUserTenantClientRequest)", ex.Message, ex.InnerException?.Message ?? string.Empty });
            }
        }

        public async Task<bool> CheckEmailAvailable(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var res = await _adminDbContext.Users.AnyAsync(x => x.Email == email);
            return !res;
        }

        public async Task<bool> CheckUsernameAvailable(string userName)
        {
            if (string.IsNullOrEmpty(userName))
                return false;

            var res = await _adminDbContext.Users.AnyAsync(x => x.UserName == userName);
            return !res;
        }
        public async Task<IResult<UserMasterResponse>> UpdateUserAsync(UpdateUserBasicInfoRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                //EN-217
                if (!string.IsNullOrEmpty(request.Email))
                {
                    if (user.Email != request.Email)
                    {
                        user.EmailConfirmed = false;

                    }
                    else
                    {
                        user.EmailConfirmed = request.EmailConfirmed; //EN-147
                    }
                }
                user.UserName = request.UserName;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber.HasValue ? request.PhoneNumber.ToString() : "";
                user.IsActive = request.IsActive;

                await _userManager.UpdateAsync(user);

                return await GetMasterAsync(request.UserId);
            }
            catch (Exception ex)
            {
                return Result<UserMasterResponse>.Fail("Failed to update User Information.");
            }
        }

        /// <summary>
        /// Updates the PIN for a user identified by their user ID.
        /// The PIN is securely encrypted using the bcrypt hashing algorithm before storage.
        /// </summary>
        /// <param name="request">The request containing the user ID and the new PIN.</param>
        /// <returns>
        /// A result indicating whether the PIN update was successful.
        /// If successful, the result contains a success message; otherwise, an error message is provided.
        /// </returns>
        public async Task<IResult> UpdatePinAsync(UpdatePinRequest request)
        {
            // Retrieve the user based on the provided user ID
            var user = await _userManager.FindByIdAsync(request.UserId);

            // If the user does not exist, return an error result
            if (user == null)
            {
                return await Result.FailAsync(_localizer["An error has occurred!"]);
            }

            // Encrypt the provided PIN using the bcrypt hashing algorithm
            var hashedPin = PinExtensions.EncryptPin(request.Pin);

            // Update the user's PIN with the securely encrypted version
            user.Pin = hashedPin; // Assuming you have a Pin property in your user model

            // Save the changes to the user
            var result = await _userManager.UpdateAsync(user);

            // Return a result indicating the success or failure of the PIN update
            return result.Succeeded
                ? await Result.SuccessAsync(_localizer["PIN changed successfully"])
                : await Result.FailAsync(_localizer["An error has occurred!"]);
        }


        public async Task<PaginatedResult<GetByUserIdQueryResponse>> GetByUserIdAsync(GetByUserIdQuery request)
        {
            try
            {
                var specification = new ClientsByUserIdSpecification(request.UserId);
                Expression<Func<UserClient, GetByUserIdQueryResponse>> expression = e => _mapper.Map<GetByUserIdQueryResponse>(e);

                //var clientRepo = _tenantRepositoryFactory.GetUserClientRepository(_currentUserService.TenantIdentifier);
                return await _userClientRepository.UserClients
                    .Specify(specification)
                    .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IResult> CreateNewUserPassword(ChangeNewUserPasswordRequest request)
        {
            //var user = await GetUserAndPreviousPasswordsAsync(request.UserId);
            var userData = await _userManager.FindByIdAsync(request.UserId);
            if (userData == null)
                return Result.Fail("User not found");

            var validation = await ValidateNewPasswordAsync(userData.Id, request.Password);

            if (!validation)
            {
                return Result.Fail("Cannot use same password");
            }

            await _userManager.RemovePasswordAsync(userData);

            var result = await _userManager.AddPasswordAsync(userData, request.Password);


            if (result.Succeeded)
            {   //EN-178
                var hasPassword = _passwordHasher.HashPassword(userData, request.Password);
                await _userUserPasswordService.RecordUserPasswordAsync(userData.Id, hasPassword);

                var greeting = GreetingHelpers.GetAppropriateGreetingString();
                var confirmAccountModel = new ConfirmAccountEmailViewModel()
                {
                    Email = userData.Email,
                    FirstName = userData.FirstName,
                    Greeting = greeting
                };
                var path = "/Views/Emails/ConfirmAccount/ResetPasswordEmail.cshtml";
                string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, confirmAccountModel);

                var mailRequest = new MailRequest()
                {
                    Body = body,
                    Subject = _localizer["Password Reset Successfully"],
                    To = userData.Email
                };

                BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
                return Result.Success("Password updated successfully");
            }

            return Result.Fail("Failed to update password");
        }

        //EN-146
        public async Task<IResult> ResendEmailConfirmation(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail("User not found");
            }
            var verificationUri = await SendVerificationEmail(user, code);
            var greeting = GreetingHelpers.GetAppropriateGreetingString();
            var confirmAccountModel = new ConfirmAccountEmailViewModel()
            {
                Email = user.Email,
                ConfirmEmailUrl = verificationUri,
                FirstName = user.FirstName,
                Greeting = greeting
            };
            var path = "/Views/Emails/ConfirmAccount/ConfirmAccountEmail.cshtml";
            string body = await _razorViewToStringRenderer.RenderViewToStringAsync(path, confirmAccountModel);

            var mailRequest = new MailRequest
            {
                From = "no-reply@automatedintegrationtechnologies.com",
                To = user.Email,
                Body = body,
                Subject = _localizer["AIT - Confirm Email"]
            };

            BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
            return await Result<string>.SuccessAsync(user.Id, message: _localizer[$"Confirmation mail is sent to user email."]);
        }

        //EN-178
        public async Task<IResult<ApplicationUser>> GetUserAndPreviousPasswordsAsync(string userId, string email = null)
        {
            try
            {
                ApplicationUser user;

                if (!string.IsNullOrEmpty(userId))
                {
                    // Search by user ID
                    user = await _userManager.Users.Include(u => u.UsedPasswords).Where(u => u.Id == userId).FirstOrDefaultAsync();
                }
                else if (!string.IsNullOrEmpty(email))
                {
                    // Search by email
                    user = await _userManager.Users.Include(u => u.UsedPasswords).Where(u => u.Email == email).FirstOrDefaultAsync();
                }
                else
                {
                    // Both identifier and email are null or empty
                    return await Result<ApplicationUser>.FailAsync("Both user ID and email are null or empty.");
                }

                if (user != null)
                {
                    return await Result<ApplicationUser>.SuccessAsync(user);
                }
                else
                {
                    return await Result<ApplicationUser>.FailAsync("User not found");
                }
            }
            catch (Exception ex)
            {
                return await Result<ApplicationUser>.FailAsync($"Error getting user: {ex.Message}");
            }
        }

        //EN-178
        private async Task<bool> ValidateNewPasswordAsync(string userId, string newPassword)
        {
            try
            {
                var user = await _userManager.Users.Include(u => u.UsedPasswords).Where(u => u.Id == userId).FirstOrDefaultAsync();

                if (user == null || user.UsedPasswords
                    .OrderByDescending(up => up.CreatedOn)
                    .Take(UsedPasswordLimit)
                    .Where(up => _passwordHasher.VerifyHashedPassword(user, up.HashedPassword, newPassword) != PasswordVerificationResult.Failed).Any())
                {
                    return false;
                }

                // Validation passed
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsUserAITSuperAdminAsync(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return false;
                }
                return await _userManager.IsInRoleAsync(user, FSHRoles.AitSuperAdmin);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IResult> UpdateUserActiveStatusAsync(StatusUpdateRequest request)
        {
            try
            {
                // Retrieve the user by user ID
                var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();

                if (user != null)
                {
                    // Check if the user is an administrator
                    var isAdmin = await _userManager.IsInRoleAsync(user, FSHRoles.Administrator);
                    if (isAdmin)
                    {
                        return await Result.FailAsync(_localizer["Administrators Profile's Status cannot be toggled"]);
                    }

                    // Process based on the user type and request parameters
                    if (user != null)
                    {
                        // Update the user's status
                        user.IsActive = request.ActivateUser;
                        var identityResult = await _userManager.UpdateAsync(user);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions gracefully
                throw; // Consider logging the exception or returning a failure result
            }

            return await Result.SuccessAsync();
        }

        public async Task SaveOTPAsync(string userId, string otp)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            // Validate OTP format (6-digit numeric)
            if (!Regex.IsMatch(otp, @"^\d{6}$"))
                throw new Exception("Invalid OTP format. OTP must be a 6-digit number.");

            // Save OTP and set expiration time (5 minutes)
            user.Otp=otp;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to save OTP.");

        }
    }
}

