using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusDashboardDenialsDetailFilterSpecification : HeroSpecification<ClaimStatusTransaction>
    {
        public ClaimStatusDashboardDenialsDetailFilterSpecification(IClaimStatusDashboardDetailsQuery filters)
        {
            Includes.Add(claimStatusTrnsaction => claimStatusTrnsaction.ClaimStatusBatchClaim);
            Criteria = claimStatusTrnsaction => true;
            Criteria = Criteria.And(claimStatusTrnsaction => !claimStatusTrnsaction.IsDeleted);
            Criteria = Criteria.And(claimStatusTrnsaction => claimStatusTrnsaction.ClaimLineItemStatusId == null || 
                        ReadOnlyObjects.ReadOnlyObjects.DeniedClaimLineItemStatuses.Contains((ClaimLineItemStatusEnum)claimStatusTrnsaction.ClaimLineItemStatusId));
            if (filters.PatientId != 0 && filters.PatientId is not null)
            {
                Criteria = Criteria.And(claimStatusTrnsaction => claimStatusTrnsaction.ClaimStatusBatchClaim.PatientId == filters.PatientId);
            }
            
        }
    }
}