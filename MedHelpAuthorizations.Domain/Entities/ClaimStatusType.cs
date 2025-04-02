using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class ClaimStatusType : AuditableEntity<ClaimStatusTypeEnum>
    {
        [StringLength(32)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
