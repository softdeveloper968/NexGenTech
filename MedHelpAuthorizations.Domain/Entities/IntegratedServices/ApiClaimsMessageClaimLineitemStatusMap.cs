using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ApiClaimsMessageClaimLineitemStatusMap : AuditableEntity<int>
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }

        [ForeignKey(nameof(ClaimLineItemStatusId))]
        public virtual ClaimLineItemStatus ClaimLineItemStatus { get; set; }
    }
}
