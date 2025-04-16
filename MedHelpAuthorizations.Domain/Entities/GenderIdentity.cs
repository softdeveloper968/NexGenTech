using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class GenderIdentity : AuditableEntity<GenderIdentityEnum>
    {
        [StringLength(50)]
        public string Name { get; set; }
    }
}
