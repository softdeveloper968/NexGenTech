using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ApprovedClaimLineItemStatusWaitPeriodSpecification : HeroSpecification<ClaimStatusBatchClaim>
	{
		public ApprovedClaimLineItemStatusWaitPeriodSpecification()
		{
			Includes.Add(bc => bc.ClaimStatusBatch.ClientInsurance.RpaInsurance);
			Criteria = bc => true;


			Criteria = bc => (bc.ClaimStatusTransaction != null &&
							  bc.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Approved)
							  ? (!bc.ClaimStatusTransaction.LastModifiedOn.HasValue || bc.ClientInsurance.RpaInsurance != null ? bc.ClaimStatusTransaction.LastModifiedOn.Value.Date.AddDays(bc.ClientInsurance.RpaInsurance.ApprovalWaitPeriodDays) <= DateTime.UtcNow : false)
							  : true;
		}
	}
}
