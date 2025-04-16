using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientLocationSpeciality : AuditableEntity<int>
    {
        public SpecialtyEnum SpecialityId { get; set; }
        public int ClientLocationId { get; set; }
        public int ClientId { get; set; }

        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
