using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class UpdateProfilePictureRequest : UploadRequest
    {
        public bool DeleteUserProfileImage { get; set; } = false;
    }
}