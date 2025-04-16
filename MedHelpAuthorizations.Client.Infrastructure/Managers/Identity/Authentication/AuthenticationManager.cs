using Blazored.LocalStorage;
using Blazored.SessionStorage;
using MedHelpAuthorizations.Client.Infrastructure.Authentication;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Requests.Identity;
using MedHelpAuthorizations.Shared.Responses.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Identity.Authentication
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly PermissionStateProvider _permissionStateProvider;
        private ClientInfoStateProvider _clientInfoStateProvider;
        private ISessionStorageService _sessionStorageService;
        private UserSessionState _userSessionState;
        private readonly IJSRuntime _jsRuntime;

        public AuthenticationManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public AuthenticationManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            ClientInfoStateProvider clientInfoStateProvider,
            ISessionStorageService sessionStorageService,
            PermissionStateProvider permissionStateProvider,
            UserSessionState userSessionState,
             IJSRuntime jsRuntime
            )
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _clientInfoStateProvider = clientInfoStateProvider;
            _sessionStorageService = sessionStorageService;
            _permissionStateProvider = permissionStateProvider;
            _userSessionState = userSessionState;
            _jsRuntime = jsRuntime;
        }

        public async Task<ClaimsPrincipal> CurrentUser()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User;
        }

        public async Task<IResult> Login(LoginRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetLogin, model);
            var result = await response.ToResult<LoginResponse>();
            if (result.Succeeded)
            {
                var token = result.Data.Token;
                var refreshToken = result.Data.RefreshToken;
                var userImageURL = result.Data.UserImageURL;

                if (_localStorage != null)
                {
                    await _localStorage.SetItemAsStringAsync("authToken", token);
                    await _localStorage.SetItemAsStringAsync("refreshToken", refreshToken);
                }

                if (!string.IsNullOrEmpty(userImageURL))
                {
                    await _localStorage.SetItemAsync("userImageURL", userImageURL);
                }

                if (_authenticationStateProvider != null)
                {
                    ((ApplicationStateProvider)this._authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
                }
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailAsync(result.Messages);
            }
        }

        public async Task<IResult> Logout()
        {
            var response = await _httpClient.GetAsync(TokenEndpoints.Logout);
            if (response != null)
            {
				// Define the list of keys to remove
				var keysToRemove = new List<string> { "authToken", "refreshToken", "userImageURL", "clientId", "clientCode", "clientName", "applicationFeatures", "apiKeys" };

				// Remove multiple items from local storage
				await _localStorage.RemoveItemsAsync(keysToRemove).ConfigureAwait(false);
				await _sessionStorageService.RemoveItemAsync("LastActivityTime").ConfigureAwait(false);
                if (!_userSessionState.IsAutoLogout)
                {
                   _clientInfoStateProvider.SetClientInfo(new ClientInfoResponse());
                }
                ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
                _httpClient.DefaultRequestHeaders.Authorization = null;
                _permissionStateProvider.SetRoles(new Dictionary<string, List<string>>());
                return await Result.SuccessAsync();
            }
            return await Result.FailAsync("Logout request failed.");
        }


        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsStringAsync("authToken");
            var refreshToken = await _localStorage.GetItemAsStringAsync("refreshToken");

            var tokenRequest = JsonSerializer.Serialize(new TokenResponse { Token = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Routes.TokenEndpoints.Refresh, bodyContent);

            var result = await response.ToResult<TokenResponse>();

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Something went wrong during the refresh token action");
            }

            token = result.Data.Token;
            refreshToken = result.Data.RefreshToken;
            await _localStorage.SetItemAsStringAsync("authToken", token);
            await _localStorage.SetItemAsStringAsync("refreshToken", refreshToken);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return token;
        }

        public async Task<string> TryRefreshToken()
        {
            try
            {
                //check if token exi
                var availableToken = await _localStorage.GetItemAsync<string>("refreshToken");
                if (string.IsNullOrEmpty(availableToken)) return string.Empty;
                var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
                var user = authState.User;
                var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
                var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
                var timeUTC = DateTime.UtcNow;
                var diff = expTime - timeUTC;
                if (diff.TotalMinutes <= 2)
                    return await RefreshToken();
                return string.Empty;
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }

        public async Task<string> TryForceRefreshToken()
        {
            return await RefreshToken();
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return await _localStorage.ContainKeyAsync("authToken");
        }

        public async Task<Dictionary<string, List<string>>> GetRolesAndPermissions()
        {
            var rolePermissionReponse = await _httpClient.GetFromJsonAsync<RolePermissionResponse>(TokenEndpoints.RolesAndPermissions);
            return rolePermissionReponse.RolesPermission;
        }

        //public async Task LoadApplicationFeatures()
        //{
        //    var applicationFeatures = await _tenantHttpClient.GetFromJsonAsync<Result<string[]>>(ApplicationEndpoints.GetApplicationFeatures);
        //    await _localStorage.SetItemAsync("applicationFeatures", JsonSerializer.Serialize(applicationFeatures.Data));
        //}

        //public async Task LoadApiKeys()
        //{
        //    var apiKeys = await _tenantHttpClient.GetFromJsonAsync<Result<ApiInegrationKeys[]>>(ApplicationEndpoints.GetApiKeys);
        //    await _localStorage.SetItemAsync("apiKeys", JsonSerializer.Serialize(apiKeys.Data));
        //}

        public async Task ClearCookiesAndSession()//EN-759
        {
            // Clear all cookies
            var clearCookiesJs = @"
                            document.cookie.split(';').forEach(cookie => {
                                document.cookie = cookie
                                    .replace(/^ +/, '')
                                    .replace(/=.*/, '=;expires=Thu, 01 Jan 1970 00:00:00 GMT;path=/');
                            });";
            await _jsRuntime.InvokeVoidAsync("eval", clearCookiesJs);

            // Clear session storage
            await _jsRuntime.InvokeVoidAsync("sessionStorage.clear");
        }

    }
}