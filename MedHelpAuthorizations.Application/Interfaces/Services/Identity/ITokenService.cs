using MedHelpAuthorizations.Application.Features.Admin.Dashboard.Models;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Shared.Requests.Identity;
using MedHelpAuthorizations.Shared.Requests.LoginHistoryRequest;
using MedHelpAuthorizations.Shared.Responses.Identity;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);
        Task<Result<LoginResponse>> LoginAsync(LoginRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);

        Task<Result<RolePermissionResponse>> GetRolesAndPermissions();

        Task<Result<ClientInfoResponse>> GetClientInfoResponse();

        Task<Result<TenantClientResponse>> GetTenantClientString(TenantClientRequest tenantClientRequest);
        Task<Result<string>> LogoutAsync(string userId);
        Task<DateTime?> GetUserLastActivityTime(string userId);
        Task<PaginatedResult<GetUserLoginHistoryGridResponse>> GetUserLoginHistoryGrid(UserLoginHistoryRequest request);
        Task<PaginatedResult<GetTenantUsersUserLoginHistoryGridResponse>> GetUserLoginHistoryGridForTenantAsync(TenantUsersLoginHistoryRequest request);
        bool IsValidSuperAdmin();

        Task<LoginResponse> VerifyOTP(VerifyLoginRequest request);
    }
}