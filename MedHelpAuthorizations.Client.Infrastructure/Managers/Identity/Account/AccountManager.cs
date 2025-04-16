using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Identity.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;

        public AccountManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.ChangePassword, model);
            return await response.ToResult();
        }

        //EN-221
        public async Task<IResult> ChangePinAsync(ChangePinRequest model)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.ChangePin, model);
                return await response.ToResult();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.UpdateProfile, model);
            return await response.ToResult();
        }

        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            var response = await _httpClient.GetAsync(Routes.AccountEndpoints.GetProfilePicture(userId));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.AccountEndpoints.UpdateProfilePicture(userId), request);
            return await response.ToResult<string>();
        }
    }
}