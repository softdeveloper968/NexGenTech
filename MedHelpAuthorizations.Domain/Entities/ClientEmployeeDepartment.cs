using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientEmployeeDepartment : AuditableEntity<int>
    {
        public int EmployeeClientId { get; set; }
        public DepartmentEnum DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("EmployeeClientId")]
        public virtual EmployeeClient EmployeeClient { get; set; }
    }
}
