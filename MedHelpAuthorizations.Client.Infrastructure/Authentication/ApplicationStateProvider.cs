using Blazored.LocalStorage;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Constants.Permission;
using MedHelpAuthorizations.Shared.Responses.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static self_pay_eligibility_api.Shared.Constants.Permission.Permissions;

namespace MedHelpAuthorizations.Client.Infrastructure.Authentication
{
    public class ApplicationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ITenantHttpClient _tenantHttpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly PermissionStateProvider _permissionStateProvider;
        private readonly ClientInfoStateProvider _clientInfoStateProvider;
		private bool _isPopulatingClientinfo { get; set; }
        public ApplicationStateProvider(
            HttpClient httpClient,
            ITenantHttpClient tenantHttpClient,
            ILocalStorageService localStorage,
            PermissionStateProvider permissionStateContainer,
            ClientInfoStateProvider clientInfoStateProvider,
            NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _tenantHttpClient = tenantHttpClient;
            _localStorage = localStorage;
            _permissionStateProvider = permissionStateContainer;
            _clientInfoStateProvider = clientInfoStateProvider;
            NavigationManager = navigationManager;
            _isPopulatingClientinfo = false;

		}

        public void MarkUserAsAuthenticated(string userName)
        {
            var authenticatedUser = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }, "apiauth"));

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));

            NotifyAuthenticationStateChanged(authState);
        }

        public async Task<ClaimsPrincipal> GetAuthenticationStateProviderUserAsync()
        {
            ClaimsPrincipal AuthenticationStateProviderUser = new ClaimsPrincipal();
            var state = await this.GetAuthenticationStateAsync();
            AuthenticationStateProviderUser = state.User;
            return AuthenticationStateProviderUser;
        }

        public async Task PopulateClientInfoProvider(bool isUpdate = false)
        {
            try
            {
                // Check if the state needs to be populated for the first time or updated based on the isUpdate flag.
                if ((_clientInfoStateProvider.ClientInfo == null && !_clientInfoStateProvider.IsPopulatingClientinfo) && !isUpdate)
                {
                    // Set the flag to indicate that the state is being populated
                    _clientInfoStateProvider.SetIsPopulatingClientinfo(true);

                    // Fetch the client information from the API
                    var clientInfo = (await _tenantHttpClient.GetFromJsonAsync<Result<ClientInfoResponse>>(TokenEndpoints.ClientInfo)).Data;

                    // Update the main client info state
                    _clientInfoStateProvider.SetClientInfo(clientInfo);

                    // Reset the flag to indicate that the population is complete
                    _clientInfoStateProvider.SetIsPopulatingClientinfo(false);
                }
                else if (isUpdate)
                {
                    //If isUpdate is true, we fetch the client information to update the state.This is necessary because we retrieve the AutoLogout time from the state and also gather the API keys required to access the database for other projects. When the client is updated, we ensure that the state is also updated accordingly.
                    ClientInfoResponse clientInfo = (await _tenantHttpClient.GetFromJsonAsync<Result<ClientInfoResponse>>(TokenEndpoints.ClientInfo))?.Data ?? null;

                    // Only update the state if new client info is successfully fetched
                    if (clientInfo != null)
                    {
                        _clientInfoStateProvider.UpdateClientInfo(clientInfo, true);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ensure the population flag is reset in case of an exception
                _clientInfoStateProvider.SetIsPopulatingClientinfo(false);

                // Rethrow the exception to notify the caller
                throw;
            }
        }

        public async Task<int> GetAuthenticationStateProviderClientAsync()
        {
            return await Task.FromResult<int>(_clientInfoStateProvider.ClientInfo.ClientId);
        }

        public ClaimsPrincipal AuthenticationStateUser { get; set; }
        public NavigationManager NavigationManager { get; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var savedToken = await _localStorage.GetItemAsStringAsync("authToken");

                if (string.IsNullOrWhiteSpace(savedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);

				if (_permissionStateProvider.RolesPermissions.Count == 0)
                {
                    var rolesPermissionResponse = await _httpClient.GetFromJsonAsync<Result<RolePermissionResponse>>(TokenEndpoints.RolesAndPermissions);
                    _permissionStateProvider.SetRoles(rolesPermissionResponse.Data.RolesPermission);
                }

                if (HasTenantClientString())
                {
                    await PopulateClientInfoProvider();
                }

                var claimIdentity = new ClaimsPrincipal();
                claimIdentity.AddIdentity(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt"));
                claimIdentity.AddIdentity(new ClaimsIdentity(GetFromResponse(_permissionStateProvider.RolesPermissions)));
                var state = new AuthenticationState(claimIdentity);
                AuthenticationStateUser = state.User;
                return state;
            }
            catch (Exception ex)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

        }

        private IEnumerable<Claim> GetFromResponse(Dictionary<string, List<string>> rolesPermissions)
        {
            var result = new List<Claim>();

            result.AddRange(rolesPermissions.Keys.Select(x => new Claim(ClaimTypes.Role, x)));

            foreach (var role in rolesPermissions.Keys)
            {
                var permissions = rolesPermissions[role];
                result.AddRange(permissions.Select(x => new Claim(ApplicationClaimTypes.Permission, x)));
            }

            return result;
        }

        private IEnumerable<Claim> GetClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);
            if (permissions != null)
            {
                if (permissions.ToString().Trim().StartsWith("["))
                {
                    var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
                    claims.AddRange(parsedPermissions.Select(permission => new Claim(ApplicationClaimTypes.Permission, permission)));
                }
                else
                {
                    claims.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
                }
                keyValuePairs.Remove(ApplicationClaimTypes.Permission);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }

        //public async Task<string> GetCurrentUserId()
        //{
        //    var user = await GetAuthenticationStateProviderUserAsync();
        //    return user.GetUserId();
        //}

        public string GetAuthenticationToken()
        {
            return _localStorage.GetItemAsStringAsync("authToken").Result;
        }

        public bool HasTenantClientString()
        {
            string queryString = new Uri(NavigationManager.Uri).Query;
            return QueryHelpers.ParseQuery(queryString).ContainsKey("t");
        }

	}
}