using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class AuthorizationClientCptCode : AuditableEntity<int>
    {
        public int AuthorizationId { get; set; }
        public int ClientCptCodeId { get; set; }

        [ForeignKey("ClientCptCodeId")]
        public virtual ClientCptCode ClientCptCode { get; set; }

        [ForeignKey("AuthorizationId")]
        public virtual Authorization Authorization { get; set; }
    }
}
