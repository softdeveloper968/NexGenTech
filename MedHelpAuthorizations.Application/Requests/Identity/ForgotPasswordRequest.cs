using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}