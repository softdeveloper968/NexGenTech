using MedHelpAuthorizations.Application.Features.Administration.EmployeeDepartments;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeRoleClaimStatusExceptionReasonCategories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeRoles
{
    public class EmployeeRoleDto
    {
        public EmployeeRoleEnum Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EmployeeLevelEnum EmployeeLevel { get; set; }
        public List<EmployeeRoleClaimStatusExceptionReasonCategoryDto> EmployeeRoleClaimStatusExceptionReasonCategories { get; set; }
        public List<EmployeeRoleDepartmentDto> EmployeeRoleDepartments { get; set; }
    }
}
