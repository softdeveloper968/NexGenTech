using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class ChangePinRequest
    {
        [Required]
        public string Pin { get; set; }

        [Required]
        public string NewPin { get; set; }

        [Required]
        public string ConfirmNewPin { get; set; }
    }
}
