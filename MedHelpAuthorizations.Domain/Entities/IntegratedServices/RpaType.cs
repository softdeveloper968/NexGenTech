using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Common.Contracts;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class RpaType : AuditableEntity<RpaTypeEnum>
    {
        public RpaType() { }
        public RpaType(RpaTypeEnum id, string code, string description, string releaseKey) 
        {
            Id = id;
            Code = code;
            Description = description;
            ReleaseKey = releaseKey;
        }

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(80)]
        public string Description { get; set; }

        public string ReleaseKey { get; set; }

        public bool IsMaxConsecutiveIssueResolved { get; set; } = true;
    }
}
