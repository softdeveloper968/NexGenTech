using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace MedHelpAuthorizations.Domain.Entities
{
    public class ApplicationReport : AuditableEntity<ApplicationReportEnum>
    {
        public ApplicationReport()
        {
            ClientUserApplicationReports = new HashSet<ClientUserApplicationReport>();
        }

        public ApplicationReport(ApplicationReportEnum id, string reportName, ApplicationFeatureEnum featureId) 
        { 
            Id = id;
            ReportName = reportName;    
            ApplicationFeatureId = featureId;
        }       

        public ApplicationFeatureEnum ApplicationFeatureId { get; set; }
        
        public string ReportName { get; set; }       


        [ForeignKey("ApplicationFeatureId")]
        public virtual ApplicationFeature ApplicationFeature { get; set; }

        public virtual ICollection<ClientUserApplicationReport> ClientUserApplicationReports { get; set; }
    }
}
