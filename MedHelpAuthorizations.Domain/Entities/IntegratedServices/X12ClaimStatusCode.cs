using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class X12ClaimStatusCode : AuditableEntity<int>
    {
        public X12ClaimStatusCode(string code, string description, ClaimLineItemStatusEnum? claimLineItemStatusId)
        {
            Code = code;
            Description = description;
            ClaimLineItemStatusId = claimLineItemStatusId;
        }
        public string Code { get; set; }
        public string Description { get; set; }
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }

        public ClaimStatusExceptionReasonCategoryEnum? ClaimStatusExceptionReasonCategoryId { get; set; }

        [ForeignKey(nameof(ClaimLineItemStatusId))]
        public virtual ClaimLineItemStatus ClaimLineItemStatus { get; set; }

        [ForeignKey(nameof(ClaimStatusExceptionReasonCategoryId))]
        public virtual ClaimStatusExceptionReasonCategory ClaimStatusExceptionReasonCategory { get; set; }
    }
}
