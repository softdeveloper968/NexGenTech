using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class AlphaSplit : AuditableEntity<AlphaSplitEnum>
    {
        [StringLength(32)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(16)]
        public string Code { get; set; }

        [StringLength(1)]
        public string BeginAlpha { get; set; }

        [StringLength(1)]
        public string EndAlpha { get; set; }
    }
}
