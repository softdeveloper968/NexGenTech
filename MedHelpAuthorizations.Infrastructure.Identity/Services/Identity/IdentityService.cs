using Finbuckle.MultiTenant;
using HttpClientToCurl;
using MedHelpAuthorizations.Application;
using MedHelpAuthorizations.Application.Configurations;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.Dashboard.Models;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Multitenancy;
using MedHelpAuthorizations.Application.Requests.Mail;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Infrastructure.Identity.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Requests.Identity;
using MedHelpAuthorizations.Shared.Requests.LoginHistoryRequest;
using MedHelpAuthorizations.Shared.Responses.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;
using IClientRepository = MedHelpAuthorizations.Application.Interfaces.Repositories.IClientRepository;
using IUserClientRepository = MedHelpAuthorizations.Application.Interfaces.Repositories.IUserClientRepository;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class IdentityService : ITokenService
    {
        private const string InvalidErrorMessage = "Invalid email or password.";

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppConfiguration _appConfig;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<IdentityService> _localizer;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ITenantManagementService _tenantManagementService;
        private readonly ITenantCryptographyService _tenantCryptographyService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ITenantInfo _tenantInfo;
        private readonly IMultiTenantStore<AitTenantInfo> _multiTenantStore;
        private readonly AdminDbContext _adminDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApiKeyValidationService _apiKeyValidationService;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public IdentityService
        (
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<AppConfiguration> appConfig,
            SignInManager<ApplicationUser> signInManager,
            IStringLocalizer<IdentityService> localizer,
            ITenantRepositoryFactory tenantRepositoryFactory,
            ITenantManagementService tenantManagementService,
            ITenantCryptographyService tenantCryptographyService,
            ICurrentUserService currentUserService,
            ITenantInfo tenantInfo,
            IMultiTenantStore<AitTenantInfo> multiTenantStore,
            AdminDbContext adminDbContext,
            IHttpContextAccessor httpContextAccessor,
            IApiKeyValidationService apiKeyValidationService,
            IConfiguration configuration,
            IMailService mailService
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appConfig = appConfig.Value;
            _signInManager = signInManager;
            _localizer = localizer;
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _tenantManagementService = tenantManagementService;
            _tenantCryptographyService = tenantCryptographyService;
            _currentUserService = currentUserService;
            _tenantInfo = tenantInfo;
            _multiTenantStore = multiTenantStore;
            _adminDbContext = adminDbContext;
            _httpContextAccessor = httpContextAccessor;
            _apiKeyValidationService = apiKeyValidationService;
            _configuration = configuration;
            _mailService = mailService;
        }

        public async Task<Result<TokenResponse>> LoginAsync(TokenRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);
            }

            IUserClientRepository userClientRepository = _tenantRepositoryFactory.GetUserClientRepository(model.TenantIdentifier);

            IClientRepository clientRepository = _tenantRepositoryFactory.GetClientRepository(model.TenantIdentifier);

            var isAllowed = await userClientRepository.VerifyUserAllowedForClient(user.Id, model.ClientCode);

            //var clientid = (await _userClientRepository.GetClientForUser(user.Id)).Id;

            var client = await clientRepository.GetByClientCode(model.ClientCode);

            if (!isAllowed)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Allowed."]);
            }

            if (!user.IsActive)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["User Not Active. Please contact the administrator."]);
            }
            if (!user.EmailConfirmed)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["E-Mail not confirmed."]);
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return await Result<TokenResponse>.FailAsync(_localizer["Invalid Credentials."]);
            }

            string tenantClientString = _tenantCryptographyService.Encrypt(model.TenantIdentifier, client.Id);

            //  Check if the refresh token is expired or not
            bool isTokenExpired = user.RefreshTokenExpiryTime <= DateTime.UtcNow;

            if (isTokenExpired)
            {
                // Send OTP only if the refresh token has expired
                string otp = new Random().Next(100000, 999999).ToString();

                // Send OTP to the user's email
                await SendOtpEmail(model.Email, otp);

                // Save OTP  
                 await SaveOTPAsync(user.Id, otp);

                // Generate a new refresh token
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(1500);
                await _userManager.UpdateAsync(user);
            }


            var token = await GenerateJwtAsync(user, client);

            var response = new TokenResponse
            {
                Token = token,
                RefreshToken = user.RefreshToken,
                UserImageURL = user.ProfilePictureDataUrl,
                TenantClientString = tenantClientString,
                TenantIdentifier = model.TenantIdentifier,
                //ApplicationFeatures = client.ClientApplicationFeatures.Select(x => x.ApplicationFeatureId.ToString()).ToArray(),
                //ApiKeys = client.ClientApiIntegrationKeys.Select(x => new ApiIntegrationKey() { ApiIntegrationName = x.ApiIntegrationId.ToString(), ApiKey = x.ApiKey }).ToArray()
            };
            return await Result<TokenResponse>.SuccessAsync(response);
        }
        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return await Result<LoginResponse>.FailAsync(_localizer["User Not Found."]);
            }
            if (!user.IsActive)
            {
                return await Result<LoginResponse>.FailAsync(_localizer["User Not Active. Please contact the administrator."]);
            }
            if (!user.EmailConfirmed)
            {
                return await Result<LoginResponse>.FailAsync(_localizer["E-Mail not confirmed."]);
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return await Result<LoginResponse>.FailAsync(_localizer["Invalid Credentials."]);
            }

            //  Check if the refresh token is expired or not
            bool isTokenExpired = user.RefreshTokenExpiryTime <= DateTime.UtcNow;

            if (isTokenExpired)
            {
                // Send OTP only if the refresh token has expired
                string otp = new Random().Next(100000, 999999).ToString();

                // Send OTP to the user's email
                await SendOtpEmail(model.Email, otp);

                // Save OTP  
                await SaveOTPAsync(user.Id, otp);

                // Generate a new refresh token
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(1500);
                await _userManager.UpdateAsync(user);
            }

            _adminDbContext.Add(new UserLoginHistory
            {
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
            });
            await _adminDbContext.SaveChangesAsync();

            var token = string.Empty;

            if (!isTokenExpired)
            {
                 token = await GenerateJwtAsync(user);
            }
            var response = new LoginResponse
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = user.RefreshToken,
                UserImageURL = user.ProfilePictureDataUrl,
            };
            return await Result<LoginResponse>.SuccessAsync(response);
        }

        /// <summary>
        /// Logs out the user identified by the provided user ID. //EN-280
        /// </summary>
        /// <param name="userId">The ID of the user to log out.</param>
        /// <returns>A task representing the asynchronous operation, returning a Result containing a string message indicating the outcome of the operation.</returns>
        public async Task<Result<string>> LogoutAsync(string usedId)
        {
            if (usedId == null)
            {
                return await Result<string>.FailAsync(_localizer["User Id not found"]);
            }
            else
            {
                var user = await _userManager.FindByIdAsync(usedId);

                if (user == null)
                {
                    return await Result<string>.FailAsync(_localizer["Error"]);
                }

                // Find the latest login entry for the user
                var latestLogin = await _adminDbContext.UserLoginHistory
                    .Where(x => x.UserId == user.Id)
                    .OrderByDescending(x => x.CreatedOn)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

                if (latestLogin != null)
                {
                    // Update the latest login entry with the current logout time
                    latestLogin.LogoutTime = DateTime.UtcNow;
                    _adminDbContext.Update(latestLogin);
                }
                else
                {
                    // Handle case where no login entry is found
                    return await Result<string>.FailAsync(_localizer["No login entry found for the user."]);
                }

                await _adminDbContext.SaveChangesAsync();

                //await _userManager.UpdateAsync(user);

                return await Result<string>.SuccessAsync();
            }

        }

        public async Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model)
        {
            try
            {
                if (model is null)
                {
                    return await Result<TokenResponse>.FailAsync(_localizer["Invalid Client Token."]);
                }
                var userPrincipal = GetPrincipalFromExpiredToken(model.Token);
                var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.FindByEmailAsync(userEmail);
                var clientId = int.Parse(userPrincipal.FindFirstValue("ClientId"));

                // var client = await _clientRepository.GetById(clientId);
                //  var client = "";
                if (user == null)
                    return await Result<TokenResponse>.FailAsync(_localizer["User Not Found."]);
                if (user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                    return await Result<TokenResponse>.FailAsync(_localizer["Invalid Client Token."]);
                var token = "";// GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, client));
                user.RefreshToken = GenerateRefreshToken();
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(1500);
                await _userManager.UpdateAsync(user);

                var response = new TokenResponse { Token = token, RefreshToken = user.RefreshToken, RefreshTokenExpiryTime = user.RefreshTokenExpiryTime };
                return await Result<TokenResponse>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<TokenResponse>.FailAsync(ex.Message);
            }
        }

        private async Task<string> GenerateJwtAsync(ApplicationUser user, Domain.Entities.Client client)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user, client));
            return token;
        }
        private async Task<string> GenerateJwtAsync(ApplicationUser user)
        {
            var token = GenerateEncryptedToken(GetSigningCredentials(), await GetClaimsAsync(user));
            return token;
        }
        private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var lastTimeLogin = await GetUserLastActivityTime(user.Id);


            var claims = new List<Claim>
                {
                    new(ClaimTypes.NameIdentifier, user.Id),
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.FirstName),
                    new(ClaimTypes.Surname, user.LastName),
                    new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty)
                };

            // Check if lastTimeLogin has a value before adding it as a claim
            //if (lastTimeLogin.HasValue)
            //{
            //	claims.Add(new Claim(ClaimTypes.Expiration, lastTimeLogin.Value.ToString()));
            //}

            claims.AddRange(userClaims);

            return claims;

        }

        private async Task<IEnumerable<Claim>> GetClaimsAsync(ApplicationUser user, Domain.Entities.Client client)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier,user.Id),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber ?? string.Empty),
                new("ClientId", client.Id.ToString())
            }
            .Union(userClaims);

            return claims;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private string GenerateEncryptedToken(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(1500),
               signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);
            return encryptedToken;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RoleClaimType = ClaimTypes.Role,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException(_localizer["Invalid token"]);
            }

            return principal;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = Encoding.UTF8.GetBytes(_appConfig.Secret);
            return new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256);
        }

        public async Task<Result<RolePermissionResponse>> GetRolesAndPermissions()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);

            if (user == null)
            {
                return Result<RolePermissionResponse>.Fail("Cannot find the user.");
            }
            var response = new RolePermissionResponse();
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                var thisRole = await _roleManager.FindByNameAsync(role);
                if (thisRole != null)
                {
                    var allPermissionsForThisRoles = (await _roleManager.GetClaimsAsync(thisRole))
                                                            .Select(x => x.Value).ToList();

                    response.RolesPermission.Add(thisRole.Name, allPermissionsForThisRoles);
                }

            }
            return Result<RolePermissionResponse>.Success(response);
        }

        public async Task<Result<ClientInfoResponse>> GetClientInfoResponse()
        {
            IClientRepository clientRepository = _tenantRepositoryFactory.GetClientRepository(_tenantInfo?.Identifier ?? string.Empty);
            var client = await clientRepository.GetSelectedClientInfoById(_currentUserService.ClientId);
            if (client == null)
            {
                return Result<ClientInfoResponse>.Fail("Cannot find the client");
            }
            var response = new ClientInfoResponse();
            response.ClientName = client.Name;
            response.ClientCode = client.ClientCode;
            response.ClientId = client.Id;
            response.TenantIdentifier = _tenantInfo?.Identifier ?? string.Empty;
            response.ApplicationFeatureIds = client.ClientApplicationFeatures.Select(x => x.ApplicationFeatureId).ToList();
            response.ApiIntegrationKeys = client.ClientApiIntegrationKeys.Select(x => new ApiIntegrationKey() { ApiIntegrationName = x.ApiIntegrationId.ToString(), ApiKey = x.ApiKey }).ToList();
            response.AutoLogMinutes = client.AutoLogMinutes;

            StartupSettings startupSettings = new StartupSettings();

            _configuration.GetSection("StartupSettings")?.Bind(startupSettings);
            string envType = startupSettings.EnvironmentType;
            response.EnvironmentType = envType;

            return Result<ClientInfoResponse>.Success(response);
        }

        public async Task<Result<TenantClientResponse>> GetTenantClientString(TenantClientRequest tenantClientRequest)
        {

            var response = new TenantClientResponse();

            var tenantInfo = await _multiTenantStore.TryGetByIdentifierAsync(tenantClientRequest.TenantIdentifier);
            if (tenantInfo == null)
            {
                return Result<TenantClientResponse>.Fail(_localizer["Tenant Not Found"]);
            }

            IUserClientRepository userClientRepository = _tenantRepositoryFactory.GetUserClientRepository(tenantClientRequest.TenantIdentifier);

            IClientRepository clientRepository = _tenantRepositoryFactory.GetClientRepository(tenantClientRequest.TenantIdentifier);

             
            var isAllowed = IsInternalOperation() || await userClientRepository.VerifyUserAllowedForClient(_currentUserService.UserId, tenantClientRequest.ClientCode);

            //var clientid = (await _userClientRepository.GetClientForUser(user.Id)).Id;

            var client = await clientRepository.GetByClientCode(tenantClientRequest.ClientCode);

            if (!isAllowed)
            {
                return await Result<TenantClientResponse>.FailAsync(_localizer["User Not Allowed."]);
            }

            string tenantClientString = _tenantCryptographyService.Encrypt(tenantClientRequest.TenantIdentifier, client.Id);
            response.TenantClientString = tenantClientString;

            return Result<TenantClientResponse>.Success(response);
        }

        /// <summary>
        /// To get user Login time
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<DateTime?> GetUserLastActivityTime(string userId)
        {
            var latestLogin = await _adminDbContext.UserLoginHistory
                                .Where(x => x.UserId == userId)
                                .OrderByDescending(x => x.CreatedOn)
                                .FirstOrDefaultAsync();

            // Return the timestamp of the latest login, if available
            return latestLogin?.LoginTime ?? null;
        }
        public async Task<PaginatedResult<GetUserLoginHistoryGridResponse>> GetUserLoginHistoryGrid(UserLoginHistoryRequest request)
        {
            var aitSuperAdminUserId = await GetAitSuperAdminUserId();
            var retVal = await _adminDbContext.UserLoginHistory.Select(x => new GetUserLoginHistoryGridResponse()
            {
                Id = x.Id,
                UserId = x.UserId,
                Username = x.ApplicationUser.UserName,
                FullName = x.ApplicationUser.LastName + ", " + x.ApplicationUser.FirstName,
                Email = x.ApplicationUser.Email,
                LoginTime = x.LoginTime,
                LogoutTime = x.LogoutTime
            })
             .Where(x => x.UserId != aitSuperAdminUserId && (string.IsNullOrEmpty(request.QuickSearch) || x.FullName.Contains(request.QuickSearch) ||
             x.UserId.Contains(request.QuickSearch) || x.Username.Contains(request.QuickSearch) ||
             x.Email.Contains(request.QuickSearch) || x.LoginTime.ToString().Contains(request.QuickSearch) || (x.LogoutTime != null && x.LogoutTime.ToString().Contains(request.QuickSearch))))
             .OrderByDescending(x => x.LoginTime)
             .ToPaginatedListAsync(request.PageNumber, request.PageSize);


            return retVal;
        }

        public async Task<PaginatedResult<GetTenantUsersUserLoginHistoryGridResponse>> GetUserLoginHistoryGridForTenantAsync(TenantUsersLoginHistoryRequest request)
        {
            var aitSuperAdminUserId = await GetAitSuperAdminUserId();

            var tenantUsers = _adminDbContext.TenantUsers.Where(x => x.TenantId == request.TenantId).Select(x => x.UserId).ToList();

            var tenantName = (await _adminDbContext.Tenants.FirstAsync(x => x.Id == request.TenantId)).TenantName;

            var retVal = await _adminDbContext.UserLoginHistory.Select(x => new GetTenantUsersUserLoginHistoryGridResponse()
            {
                Id = x.Id,
                TenantName = tenantName,
                UserId = x.UserId,
                Username = x.ApplicationUser.UserName,
                FullName = x.ApplicationUser.LastName + ", " + x.ApplicationUser.FirstName,
                Email = x.ApplicationUser.Email,
                LoginTime = x.LoginTime,
                LogoutTime = x.LogoutTime
            })
             .Where(x => (x.UserId != aitSuperAdminUserId && tenantUsers.Contains(x.UserId))
             && (string.IsNullOrEmpty(request.QuickSearch) || x.FullName.Contains(request.QuickSearch) ||
             x.UserId.Contains(request.QuickSearch) || x.Username.Contains(request.QuickSearch) ||
             x.Email.Contains(request.QuickSearch) || x.LoginTime.ToString().Contains(request.QuickSearch) || (x.LogoutTime != null && x.LogoutTime.ToString().Contains(request.QuickSearch))))
             .OrderByDescending(x => x.LoginTime)
             .ToPaginatedListAsync(request.PageNumber, request.PageSize);


            return retVal;
        }

        public async Task<string> GetAitSuperAdminUserId()
        {
            var superAdminUser = await _userManager.Users.FirstAsync(x => x.NormalizedUserName == "AITADMIN" && x.Email == "no-reply@automatedintegrationtechnologies.com");
            return superAdminUser.Id;
        }

        public bool IsInternalOperation()
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                var httpContext = _httpContextAccessor.HttpContext;

                // Check if the API key is present in the headers
                if (httpContext.Request.Headers.TryGetValue("x-api-key", out var providedApiKey))
                {
                    // Validate the API key
                    if (_apiKeyValidationService.IsValidApiKey(providedApiKey))
                    {
                        return true;  // The request is considered internal if a valid API key is provided
                    }
                }
            }

            return false;  // Return false if no valid API key is found
        }

        public bool IsValidSuperAdmin()//EN-797
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user?.Identity?.IsAuthenticated != true)
                    return false;

                var userId = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                var userEmail = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(userEmail) ||
                    userEmail.Equals("no-reply@automatedintegrationtechnologies.com", StringComparison.OrdinalIgnoreCase))
                    return false;

                var userEntity = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();
                return userEntity != null && _userManager.IsInRoleAsync(userEntity, "AitSuperAdmin").GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SuperAdminPolicy exception : {ex.Message} {Environment.NewLine} StackTrace: {ex.StackTrace}"); 
                return false;
            }
            
        }

        public async Task SendOtpEmail(string email, string otp)
        {
            var mailRequest = new MailRequest
            {
                Subject = "Your OTP Code",
                Body = $"Your verification code is: {otp}",
                To = email
            };
            await _mailService.SendAsync(mailRequest);
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
            user.Otp = otp;
            //user.OTPExpiryTime = DateTime.UtcNow.AddMinutes(5);

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("Failed to save OTP.");
        }

        public async Task<LoginResponse> VerifyOTP(VerifyLoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("User not found.");

            // Validate OTP
            if (string.IsNullOrEmpty(request.Otp))
                throw new Exception("OTP is required.");

       

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Clear OTP after successful verification
            user.Otp = null;
           
            await _userManager.UpdateAsync(user);

            // Call the existing `GenerateJWTToken` method (assuming it expects a **tuple**)
            string token  = await GenerateJwtAsync(user);
            var response = new LoginResponse
            {
                UserId = user.Id,
                Token = token,
                RefreshToken = user.RefreshToken,
                UserImageURL = user.ProfilePictureDataUrl,
            };
            return response;

            
        }

    }
}