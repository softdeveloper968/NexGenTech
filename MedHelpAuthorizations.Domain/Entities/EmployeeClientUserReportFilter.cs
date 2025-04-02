using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeClientUserReportFilter: AuditableEntity<int>
    {
        public EmployeeClientUserReportFilter() { }
        /// <summary>
        /// Client User Report Filter report Id [Original ReportId]
        /// </summary>
        public int ClientUserReportFilterId { get; set; }
        /// <summary>
        /// Shared Employee Id
        /// </summary>
        public int EmployeeClientId { get; set; }
        /// <summary>
        /// When any report is “saved as”.. it will record the Id of the base report (BaseReportId).
        /// user will allow to CRUD.
        /// </summary>
        public int? BaseClientUserReportFilterId { get; set; }=null;

         [ForeignKey("ClientUserReportFilterId")]
        public virtual ClientUserReportFilter ClientUserReportFilter { get; set; }

         [ForeignKey("EmployeeClientId")]
        public virtual EmployeeClient EmployeeClient { get; set; }

    }
}
