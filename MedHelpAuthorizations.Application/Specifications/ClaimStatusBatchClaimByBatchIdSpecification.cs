using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ClaimStatusBatchClaimByBatchIdSpecification : HeroSpecification<ClaimStatusBatchClaim>
	{
		/// <summary>
		/// Represents a specification used to filter ClaimStatusBatchClaims by their associated ClaimStatusBatchId.
		/// </summary>
		public ClaimStatusBatchClaimByBatchIdSpecification(int claimStatusBatchId) //AA-231
		{
			Criteria = bc => true;
			Criteria = Criteria.And(c => c.ClaimStatusBatchId == claimStatusBatchId);
        }
	}
}
