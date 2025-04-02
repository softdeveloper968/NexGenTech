using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
	public class ClaimStatusBatchesIncludeAssignedSpecification : HeroSpecification<ClaimStatusBatch>
	{
		public ClaimStatusBatchesIncludeAssignedSpecification()
		{
			Criteria = batchClaim =>
			batchClaim.AssignedDateTimeUtc == null
			&& batchClaim.CompletedDateTimeUtc == null
			&& batchClaim.AbortedOnUtc == null;
		}
	}
}
