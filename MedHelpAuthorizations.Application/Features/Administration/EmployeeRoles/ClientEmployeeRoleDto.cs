using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles
{
    public class ClientEmployeeRoleDto 
    {
        public int? Id { get; set; }
        public int? EmployeeClientId { get; set; }
        public EmployeeRoleEnum EmployeeRoleId { get; set; }
        public EmployeeRoleDto EmployeeRole { get; set; }
    }
}
