using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Requests.Identity
{
    public class ToggleUserStatusRequest
    {
        public bool ActivateUser { get; set; }
        public string UserId { get; set; }
        public bool CreateEmployee { get; set; } = true; //AA-233
        public string EmployeeNumber { get; set; } //AA-233
        public EmployeeRoleEnum? DefaultEmployeeRoleId { get; set; } //AA-233
        public bool RemoveExistingEmployee { get; set; } = false; //AA-233
        public IEnumerable<string> TenantIdentifiers { get; set; } //AA-233
        public bool IsExistingEmployee { get; set; }
    }
}