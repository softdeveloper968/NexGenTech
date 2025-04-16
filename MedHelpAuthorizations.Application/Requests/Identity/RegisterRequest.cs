using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

        public long? PhoneNumber { get; set; } = null;

        public bool ActivateUser { get; set; } = true;

        public bool AutoConfirmEmail { get; set; } = true;

        public bool CreateEmployee { get; set; } = true;

        public IEnumerable<int> ClientIds { get; set; }
        public IEnumerable<string> TenantIdentifiers { get; set; }
        public string EmployeeNumber { get; set; }
        public EmployeeRoleEnum? DefaultEmployeeRoleId { get; set; }
        public bool ReceiveReport { get; set; } = true;

        [Required]
        [MinLength(5)]
        public string Pin { get; set; }

        [Required]
        [Compare(nameof(Pin))]
        public string ConfirmPin { get; set; }
    }
}