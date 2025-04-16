using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class TokenRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string ClientCode { get; set; }
    }
}