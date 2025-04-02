using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientUserReportFilter : AuditableEntity<int>, IClientRelationship, IUserRelationship, ISoftDelete
    {
        public ClientUserReportFilter()
        {
            HashSet<EmployeeClientUserReportFilter> EmployeeClientUserReportFilters = new();
        }
        public ReportsEnum ReportId { get; set; }
        public int ClientId { get; set; }
        public string UserId { get; set; }///AA-193 [As per discussion with kevin, ReportFilters are filtered by Login UserId and their respective ClientId.]
        public string FilterName { get; set; }
        public string FilterConfiguration { get; set; }//save json data
        public bool HasDefaultFilter { get; set; }
        public bool RunSavedDefaultFilter { get; set; }
        /// <summary>
        /// EN-108
        /// Add new Column/Property to ClientUSerReportFilter called “SystemDefaultReportFilterId“. Default this to null;
        /// Only Deleted From ClientUserReportFilter.| replica save in SystemDefaultReportFilter
        /// </summary>
        public int? SystemDefaultReportFilterId { get; set; } = null;
        /// <summary>
        /// Always do soft Delete.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        public ICollection<EmployeeClientUserReportFilter> EmployeeClientUserReportFilters { get; set; }

        [ForeignKey("SystemDefaultReportFilterId")]
        public virtual SystemDefaultReportFilter SystemDefaultReportFilter { get; set; }

    }
}
