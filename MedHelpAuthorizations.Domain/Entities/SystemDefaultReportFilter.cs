using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class SystemDefaultReportFilter: AuditableEntity<int>
    {
        public SystemDefaultReportFilter()
        {
            SystemDefaultReportFilterEmployeeRoles = new HashSet<SystemDefaultReportFilterEmployeeRole>();
        }

        public ReportsEnum ReportId { get; set; }
        public string FilterName { get; set; }
        public string FilterConfiguration { get; set; }//save json data
        public bool IsActive { get; set; }

        public virtual ICollection<SystemDefaultReportFilterEmployeeRole> SystemDefaultReportFilterEmployeeRoles { get; set; }

    }
}