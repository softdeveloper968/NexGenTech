using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Account;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Infrastructure.Identity.Services.Identity
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly IUploadService _uploadService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IStringLocalizer<AccountService> _localizer;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IBlobStorageService blobStorageService,
            IStringLocalizer<AccountService> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_uploadService = uploadService;
            _blobStorageService = blobStorageService;
            _localizer = localizer;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result.FailAsync(_localizer["User Not Found."]);
            }

            var identityResult = await this._userManager.ChangePasswordAsync(
                user,
                model.Password,
                model.NewPassword);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }

        public async Task<IResult> ChangePinAsync(ChangePinRequest model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            // Null check for user
            if (user == null)
            {
                return await Result.FailAsync(_localizer["User not found."]);
            }

            var decryptCurrentPin = PinExtensions.DecryptPin(user.Pin);


            if (decryptCurrentPin == null)
            {
                return await Result.FailAsync(_localizer["Current PIN not available for the user."]);
            }
            if (model.Pin != decryptCurrentPin)
            {
                return await Result.FailAsync(_localizer["Current PIN doesn't match."]);
            }
            if (model.NewPin == decryptCurrentPin)
            {
                return await Result.FailAsync(_localizer["Current PIN and New PIN can't be the same."]);
            }
            else
            {
                var encryptedNewPin = PinExtensions.EncryptPin(model.NewPin);

                user.Pin = encryptedNewPin;

                var updateResult = await _userManager.UpdateAsync(user);

                // Return a result indicating the success or failure of the PIN update
                return updateResult.Succeeded
                    ? await Result.SuccessAsync(_localizer["PIN was saved"])
                    : await Result.FailAsync(_localizer["An error has occurred during PIN change."]);
            }
        }



        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result.FailAsync(_localizer["User Not Found."]);
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            }
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            await _signInManager.RefreshSignInAsync(user);
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }

        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return await Result<string>.FailAsync(_localizer["User Not Found"]);
            if (!string.IsNullOrEmpty(user.ProfilePictureDataUrl))
            {
                var imageBase64String = await _blobStorageService.GetImageURL(user.ProfilePictureDataUrl);
                if (imageBase64String != null)
                {
                    return await Result<string>.SuccessAsync(data: imageBase64String);
                }
            }
            //return await Result<string>.SuccessAsync(data: user.ProfilePictureDataUrl);
            return await Result<string>.SuccessAsync("");
        }

        public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return await Result<string>.FailAsync(message: _localizer["User Not Found"]);
            string filePath = string.Empty;

            if (request.DeleteUserProfileImage)
            {
                request.FileName = user.ProfilePictureDataUrl;
                await _blobStorageService.DeleteBlobAsync(request.FileName);
            }
            else
            {
                filePath = _blobStorageService.UploadToBlobStorageAsync(request)?.Result ?? string.Empty;
            }
            user.ProfilePictureDataUrl = filePath;
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return identityResult.Succeeded ? await Result<string>.SuccessAsync(data: filePath) : await Result<string>.FailAsync(errors);
        }
    }
}