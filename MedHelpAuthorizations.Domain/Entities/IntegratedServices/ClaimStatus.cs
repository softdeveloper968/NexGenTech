using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ClaimStatus : AuditableEntity<ClaimStatusEnum>
    {
        public ClaimStatus() { }
        public ClaimStatus(ClaimStatusEnum id, string code, string description)
        {
            Id = id;
            Code = code;
            Description = description;
        }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(80)]
        public string Description { get; set; }
    }
}
