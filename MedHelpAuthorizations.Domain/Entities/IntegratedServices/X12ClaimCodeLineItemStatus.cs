using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
	public class X12ClaimCodeLineItemStatus : AuditableEntity<int>
	{
		public X12ClaimCodeLineItemStatus()
		{

		}
		public X12ClaimCodeLineItemStatus(string code, string description, X12ClaimCodeTypeEnum x12ClaimCodeTypeId, ClaimLineItemStatusEnum? claimLineItemStatusId, ClaimStatusExceptionReasonCategoryEnum? claimStatusExceptionReasonCategoryId)
		{
			Code = code;
			Description = description;
			X12ClaimCodeTypeId = x12ClaimCodeTypeId;
			ClaimLineItemStatusId = claimLineItemStatusId;
			ClaimStatusExceptionReasonCategoryId = claimStatusExceptionReasonCategoryId;
		}

		public string Code { get; set; }

		public string Description { get; set; }

		public X12ClaimCodeTypeEnum X12ClaimCodeTypeId { get; set; }

		public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }

		public ClaimStatusExceptionReasonCategoryEnum? ClaimStatusExceptionReasonCategoryId { get; set; }


		[ForeignKey(nameof(X12ClaimCodeTypeId))]
		public virtual X12ClaimCodeType X12ClaimCodeType { get; set; }


		[ForeignKey(nameof(ClaimLineItemStatusId))]
		public virtual ClaimLineItemStatus ClaimLineItemStatus { get; set; }


		[ForeignKey(nameof(ClaimStatusExceptionReasonCategoryId))]
		public virtual ClaimStatusExceptionReasonCategory ClaimStatusExceptionReasonCategory { get; set; }
	}
}
