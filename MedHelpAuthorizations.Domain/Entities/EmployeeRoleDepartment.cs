using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeRoleDepartment : AuditableEntity<int>
    {
        public EmployeeRoleEnum EmployeeRoleId { get; set; }
        public DepartmentEnum DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("EmployeeRoleId")]
        public virtual EmployeeRole EmployeeRole { get; set; }
    }
}
