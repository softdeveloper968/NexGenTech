using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class SystemDefaultReportFilterEmployeeRole: AuditableEntity<int>
    {
        public SystemDefaultReportFilterEmployeeRole() { }
        public int SystemDefaultReportFilterId {  get; set; }
        public EmployeeRoleEnum EmployeeRoleId { get; set; }

         [ForeignKey("SystemDefaultReportFilterId")]
        public virtual SystemDefaultReportFilter SystemDefaultReportFilter { get; set; }

         [ForeignKey("EmployeeRoleId")]
        public virtual EmployeeRole EmployeeRole { get; set; }
    }
}
