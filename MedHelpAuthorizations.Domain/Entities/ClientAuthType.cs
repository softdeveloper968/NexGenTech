using System.ComponentModel.DataAnnotations.Schema;
using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;

#nullable disable

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientAuthType : AuditableEntity<int>//, ITenant
    {
        public int AuthTypeId { get; set; }
        public int ClientId { get; set; }
        //public string TenantId { get; set; }

        [ForeignKey("AuthTypeId")]
        public virtual AuthType AuthType { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
