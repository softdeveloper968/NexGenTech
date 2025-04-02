using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeDepartments
{
    public class EmployeeRoleDepartmentDto : IRequest<Result<int>>
    {
        public EmployeeRoleEnum EmployeeRoleId { get; set; }
        public DepartmentEnum DepartmentId { get; set; }
        //public virtual DepartmentDto Department { get; set; }
        public virtual EmployeeRoleDto EmployeeRole { get; set; }
    }
}
