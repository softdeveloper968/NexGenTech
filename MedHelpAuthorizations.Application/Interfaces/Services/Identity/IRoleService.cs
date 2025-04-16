using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Shared;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Identity
{
    public interface IRoleService : IService
    {
        Task<Result<List<RoleResponse>>> GetAllAsync();
        Task<Result<List<RoleNamesResponse>>> GetAllRolesAsync();
        Task<PaginatedResult<RoleResponse>> GetPaginatedAsync(int pageNumber, int pageSize, string search);
        Task<int> GetCountAsync();

        Task<Result<RoleResponse>> GetByIdAsync(string id);

        Task<Result<string>> SaveAsync(RoleRequest request);

        Task<Result<string>> DeleteAsync(string id);

        Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);

        Task<IResult<string>> UpdatePermissionsAsync(PermissionRequest request);
    }
}