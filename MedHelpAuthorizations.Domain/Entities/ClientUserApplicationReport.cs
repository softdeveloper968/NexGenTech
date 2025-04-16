using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientUserApplicationReport : AuditableEntity<int>//, ITenant
    {
        public int UserClientId { get; set; }
        public ApplicationReportEnum ApplicationReportId { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ApplicationReportId")]
        public virtual ApplicationReport ApplicationReport { get; set; }

        [ForeignKey("UserClientId")]
        public virtual UserClient UserClient { get; set; }    

    }
}
