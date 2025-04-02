using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }

    public class ChangeNewUserPasswordRequest
    {
        public string UserId { get; set;}
        [Required]
        public string Password { get; set; }
    }
}