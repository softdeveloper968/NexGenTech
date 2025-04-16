using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
	public class X12ClaimCategoryCodeLineItemStatus : AuditableEntity<int>
	{
		public X12ClaimCategoryCodeLineItemStatus()
		{

		}
		public X12ClaimCategoryCodeLineItemStatus(string code, string description, X12ClaimCategoryEnum x12ClaimCategoryId, ClaimLineItemStatusEnum? claimLineItemStatusId) 
		{ 
			Code = code;
			Description = description;
			X12ClaimCategoryId = x12ClaimCategoryId;
			ClaimLineItemStatusId = claimLineItemStatusId;
		}

		public string Code { get; set; }
		public string Description { get; set; }

		public X12ClaimCategoryEnum X12ClaimCategoryId { get; set; }

		public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }


		[ForeignKey(nameof(X12ClaimCategoryId))]
		public virtual X12ClaimCategory X12ClaimCategory { get; set; }


		[ForeignKey(nameof(ClaimLineItemStatusId))]
		public virtual ClaimLineItemStatus ClaimLineItemStatus { get; set; }
	}
}
