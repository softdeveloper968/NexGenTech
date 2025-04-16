using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class UserClient : AuditableEntity<int>//, ITenant
    {
        public UserClient()
        {
            ClientUserApplicationReports = new HashSet<ClientUserApplicationReport>();
        }

        public string UserId { get; set; }
        public int ClientId { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public virtual ICollection<ClientUserApplicationReport> ClientUserApplicationReports { get; set; }
    }
}
