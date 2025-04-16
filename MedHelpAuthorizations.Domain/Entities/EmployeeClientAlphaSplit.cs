using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities
{
    public class EmployeeClientAlphaSplit : AuditableEntity<int>
    {
        public int? EmployeeClientId { get; set; }

        [StringLength(2)]
        public string CustomBeginAlpha { get; set; }

        [StringLength(2)]
        public string CustomEndAlpha { get; set; }

        public AlphaSplitEnum AlphaSplitId { get; set; }

        [ForeignKey("EmployeeClientId")]
        public virtual EmployeeClient EmployeeClient { get; set; }

        [ForeignKey("AlphaSplitId")]
        public virtual AlphaSplit AlphaSplit { get; set; }
    }
}
