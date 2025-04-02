using MedHelpAuthorizations.Application.Responses.Identity;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees
{
    public class EmployeeManagerDto
    {
        public int? Id { get; set; } 
        public string EmployeeNumber { get; set; }

        public EmployeeRoleEnum? DefaultEmployeeRoleId { get; set; }

        public string UserId { get; set; } //AA-230
        public UserResponse User { get; set; } //AA-230
    }
}
