using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClientSpecialty : AuditableEntity<int>
    {
        public SpecialtyEnum SpecialtyId { get; set; }
        public int ClientId { get; set; }

        [ForeignKey("SpecialtyId")]
        public virtual Specialty Specialty { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
    }
}
