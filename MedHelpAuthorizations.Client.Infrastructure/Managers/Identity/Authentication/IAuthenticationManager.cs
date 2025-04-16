using MedHelpAuthorizations.Shared.Requests.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Identity.Authentication
{
    public interface IAuthenticationManager : IManager
    {
        Task<IResult> Login(LoginRequest model);

        Task<IResult> Logout();

        Task<string> RefreshToken();

        Task<string> TryRefreshToken();

        Task<string> TryForceRefreshToken();

        Task<ClaimsPrincipal> CurrentUser();

        Task<bool> IsUserAuthenticated();
        Task<Dictionary<string, List<string>>> GetRolesAndPermissions();
        Task ClearCookiesAndSession(); //EN-759

        //Task LoadApplicationFeatures();
        //Task LoadApiKeys();
    }
}