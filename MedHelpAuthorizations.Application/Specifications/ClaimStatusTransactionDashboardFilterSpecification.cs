using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Specifications.Base;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusTransactionDashboardFilterSpecification : ClaimStatusTransactionDashboardFilterSpecificationBase
    {
        public ClaimStatusTransactionDashboardFilterSpecification(IClaimStatusDashboardStandardQuery query, int clientId) : base(query, clientId)
        {
            if (query.ClaimStatusBatchId > 0)
            {
                Criteria = Criteria.And(claimStatusTrnsaction => claimStatusTrnsaction.ClaimStatusBatchClaim.ClaimStatusBatch.Id == query.ClaimStatusBatchId);
            }
        }
    }
}
