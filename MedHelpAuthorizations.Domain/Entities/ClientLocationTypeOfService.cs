using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientLocationTypeOfService : AuditableEntity<int>
    {
        public TypeOfServiceEnum TypeOfServiceId { get; set; }
        public int ClientLocationId { get; set; }
        public int ClientId { get; set; }

        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }

        [ForeignKey("TypeOfServiceId")]
        public virtual TypeOfService TypeOfService { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
