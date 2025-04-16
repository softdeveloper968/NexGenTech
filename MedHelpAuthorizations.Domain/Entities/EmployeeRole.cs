using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeRole : AuditableEntity<EmployeeRoleEnum>
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
        //public bool IsManager { get; set; } = false;
        //public bool IsChiefLevel { get; set; } = false;
        public EmployeeLevelEnum EmployeeLevel { get; set; }
        public virtual ICollection<EmployeeRoleClaimStatusExceptionReasonCategory> EmployeeRoleClaimStatusExceptionReasonCategories { get; set; }
        public virtual ICollection<EmployeeRoleDepartment> EmployeeRoleDepartments { get; set; }
        public virtual ICollection<ClientEmployeeRole> ClientEmployeeRole { get; set; }

    }
}
