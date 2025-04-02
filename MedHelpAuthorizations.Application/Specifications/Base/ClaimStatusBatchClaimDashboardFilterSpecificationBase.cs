using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications.Base
{
    public class ClaimStatusBatchClaimDashboardFilterSpecificationBase : HeroSpecification<ClaimStatusBatchClaim>
    {
        public ClaimStatusBatchClaimDashboardFilterSpecificationBase(IClaimStatusDashboardStandardQuery query, int clientId)
        {
            Includes.Add(c => c.ClaimStatusBatch);
            Includes.Add(c => c.ClaimStatusBatch.AuthType);

            Criteria = c => true;
            Criteria = Criteria.And(c => !c.IsDeleted);
            Criteria = Criteria.And(c => c.ClientId == clientId);

            if (!string.IsNullOrEmpty(query.ClientInsuranceIds))
            {
                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.ClientInsuranceIds, true).Contains(c.ClaimStatusBatch.ClientInsuranceId));
            }
            
            if (!string.IsNullOrEmpty(query.ClientLocationIds))
            {
                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.ClientLocationIds, true).Contains((int)c.ClientLocationId));
            }
            
            if (!string.IsNullOrEmpty(query.ClientProviderIds))
            {
                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.ClientProviderIds, true).Contains((int)c.ClientProviderId));
            }

            if (!string.IsNullOrEmpty(query.AuthTypeIds))
            {
                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertStringToList(query.AuthTypeIds, true).Contains(c.ClaimStatusBatch.AuthTypeId ?? 0));
            }

            if (!string.IsNullOrWhiteSpace(query.ProcedureCodes))
            {
                Criteria = Criteria.And(c => ClaimFiltersHelpers.ConvertProcedureCodesToList(query.ProcedureCodes).Contains(c.ProcedureCode.Trim()));
            }

            if (query.ReceivedFrom != null)
            {
                Criteria = Criteria.And(c => c.CreatedOn.Date >= query.ReceivedFrom.Value.Date);
            }
            if (query.ReceivedTo != null)
            {
                Criteria = Criteria.And(c => c.CreatedOn.Date <= query.ReceivedTo.Value.Date);
            }

            if (query.DateOfServiceFrom != null)
            {
                Criteria = Criteria.And(c => c.DateOfServiceFrom.Value.Date >= query.DateOfServiceFrom.Value.Date);
            }
            if (query.DateOfServiceTo != null)
            {
                Criteria = Criteria.And(c => c.DateOfServiceTo.Value.Date <= query.DateOfServiceTo.Value.Date);
            }

            if (query.TransactionDateFrom != null)
            {
                Criteria = Criteria.And(c => c.ClaimStatusTransaction != null && (c.ClaimStatusTransaction.LastModifiedOn == null ? c.ClaimStatusTransaction.CreatedOn.Date >= query.TransactionDateFrom.Value.Date : c.ClaimStatusTransaction.LastModifiedOn.Value.Date >= query.TransactionDateFrom.Value.Date));
            }
            if (query.TransactionDateTo != null)
            {
                Criteria = Criteria.And(c => c.ClaimStatusTransaction != null ? (c.ClaimStatusTransaction.LastModifiedOn == null ? c.ClaimStatusTransaction.CreatedOn.Date <= query.TransactionDateTo.Value.Date : c.ClaimStatusTransaction.LastModifiedOn.Value.Date <= query.TransactionDateTo.Value.Date) : true);
            }

            if (query.ClaimBilledFrom != null)
            {
                Criteria = Criteria.And(c => c.ClaimBilledOn != null && c.ClaimBilledOn.Value.Date >= query.ClaimBilledFrom.Value.Date);
            }
            if (query.ClaimBilledTo != null)
            {
                Criteria = Criteria.And(c => c.ClaimBilledOn != null && c.ClaimBilledOn.Value.Date <= query.ClaimBilledTo.Value.Date);
            }
            if (!string.IsNullOrEmpty(query.ClientLocationIds))//AA-207
            {
                Criteria = Criteria.And(c => c.ClientLocationId.HasValue && ClaimFiltersHelpers.ConvertStringToList(query.ClientInsuranceIds, true).Contains((int)c.ClientLocationId));
            }
        }
    }
}
