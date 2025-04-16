using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Features.UserClients.Queries.GetByUserId;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Identity
{
    public interface IUserService : IService
    {
        Task<Result<List<UserResponse>>> GetAllAsync();
        Task<PaginatedResult<UserInfoResponse>> GetUsersPageAsync(int pageNumber, int pageSize, string search);

        Task<int> GetCountAsync();

        Task<IResult<UserResponse>> GetAsync(string userId);
        Task<IResult<UserMasterResponse>> GetMasterAsync(string userId);
        Task<IResult> RegisterAsync(RegisterRequest request, string origin);
        Task<IResult> CreateUserAsync(CreateUserRequest request, string origin);
        Task<IResult<UserMasterResponse>> UpdateUserAsync(UpdateUserBasicInfoRequest request);

        Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);

        Task<IResult<UserRolesResponse>> GetRolesAsync(string id);
        Task<IResult<UserAllRolesResponse>> GetUserRolesAsync(string userId);

        Task<IResult> UpdateRolesAsync(string userId, UpdateUserRolesRequest request);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task<IResult> UpdatePasswordAsync(UpdatePasswordRequest request);
        Task<string> GetNameAsync(string userId);
        Task<Dictionary<string, string>> GetNamesAsync(string[] userIds);
        Task<IResult<List<UserResponse>>> GetUserForPermissionAsync(string permission);
        List<UserResponse> GetAllUserForPermission(string permission);
        Task<List<ClientUserResponse>> GetAllClientUserForApplicationReport(string permission, Domain.Entities.Enums.ApplicationReportEnum ApplicationReportId);
        Task<IResult<IEnumerable<TenantResponse>>> GetAssignedTenantsAsync();
        Task<IResult<IEnumerable<TenantResponse>>> GetAssignedTenantsByUserIdAsync(string userId); //AA-233
        Task<IResult<IEnumerable<ClientResponse>>> GetAssginedClientsAsync(string tenantIndetifier);
        Task<IResult<IEnumerable<TenantResponse>>> GetAssignedTenantsAsync(string userId);
        Task<IResult<IEnumerable<BasicTenantClientResponse>>> GetAssginedTenantsClientsAsync(string userId);
        Task<IResult> UpdateTenantClients(string userId, UpdateUserTenantClientRequest request);
        Task<IResult> UpdateClients(string userId, int tenantId, List<int> clientIds);
        Task<bool> CheckEmailAvailable(string email);
        Task<bool> CheckUsernameAvailable(string userName);
        Task<IResult> AddEmployeeToTenant(string tenantId, Employee newEmployee);
        Task<IResult> RemoveEmployeeFromTenant(ICollection<string> tenantId, string userId); //AA-233
        Task<IResult> UpdatePinAsync(UpdatePinRequest request); //EN-88
        Task<PaginatedResult<GetByUserIdQueryResponse>> GetByUserIdAsync(GetByUserIdQuery request);
        Task<string> GenerateChangePasswordLink(ApplicationUser user, string origin); //AA-104

        Task<IResult> CreateNewUserPassword(ChangeNewUserPasswordRequest request);
        Task<IResult> ResendEmailConfirmation(string userId, string code);
        Task<bool> IsUserAITSuperAdminAsync(string userId);
        Task<IResult> UpdateEmployeeTenantClients(string userId, Dictionary<int, List<int>> request);
        Task<IResult<Dictionary<int, List<int>>>> GetEmployeeTenantClients(string userId);
        Task<IResult> UpdateUserActiveStatusAsync(StatusUpdateRequest request);
    }
}