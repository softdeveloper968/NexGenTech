using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Application.Requests.Identity;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);
        Task<IResult> ChangePinAsync(ChangePinRequest model, string userId); //EN-221

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}