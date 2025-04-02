using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Specifications.Base;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchClaimDashboardFilterSpecification : ClaimStatusBatchClaimDashboardFilterSpecificationBase
    {
        public ClaimStatusBatchClaimDashboardFilterSpecification(IClaimStatusDashboardStandardQuery query, int clientId) : base(query, clientId)
        {
            if (query.ClaimStatusBatchId > 0)
            {
                Criteria = Criteria.And(c => c.ClaimStatusBatch.Id == query.ClaimStatusBatchId);
            }
            if(query.ClientLocationId > 0)
            {
                Criteria = Criteria.And(c => c.ClientLocationId== query.ClientLocationId);
            }
            if(query.ClientProviderId > 0)
            {
                Criteria = Criteria.And(c => c.ClientProviderId == query.ClientProviderId);
            }
        }
    }
}
