using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusTransactionNotSupplantedFilterSpecification : HeroSpecification<ClaimStatusTransaction>
	{
        public ClaimStatusTransactionNotSupplantedFilterSpecification()
        {
            Includes.Add(claimStatusTrnsaction => claimStatusTrnsaction.ClaimStatusBatchClaim);

            Criteria = claimStatusTrnsaction => true;

            Criteria = Criteria.And(claimStatusTrnsaction => claimStatusTrnsaction.ClaimStatusBatchClaim != null ? !claimStatusTrnsaction.ClaimStatusBatchClaim.IsSupplanted : true);
			Criteria = Criteria.And(claimStatusTrnsaction => claimStatusTrnsaction.ClaimStatusBatchClaim != null ? !claimStatusTrnsaction.ClaimStatusBatchClaim.IsDeleted : true);
		}
	}
}
