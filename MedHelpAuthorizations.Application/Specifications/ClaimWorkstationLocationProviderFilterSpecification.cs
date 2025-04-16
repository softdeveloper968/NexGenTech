using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimWorkstationLocationProviderFilterSpecification : HeroSpecification<ClaimStatusTransaction>
    {
        public ClaimWorkstationLocationProviderFilterSpecification(IClaimWorkstationDetailQuery query, int clientId, List<int> insuranceIds, List<ClaimStatusExceptionReasonCategoryEnum> exceptionReasonIds, List<int> serviceTypeIds, List<string> procedureCodes, List<int> locationIds, List<int> providerIds)
        {
            Includes.Add(t => t.ClaimStatusBatchClaim);
            Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch);
            Includes.Add(t => t.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsurance);
            Includes.Add(t => t.ClaimStatusBatchClaim.ClientLocation);
            Includes.Add(t => t.ClaimStatusBatchClaim.ClientProvider);

            Criteria = p => true;
            Criteria = c => !c.IsDeleted && c.ClientId == clientId;

            ///Check for client Insurances.
            if (!string.IsNullOrEmpty(query.ClientInsuranceIds))//AA-120
            {
                Criteria = Criteria.And(c => insuranceIds.Contains(c.ClaimStatusBatchClaim.ClaimStatusBatch.ClientInsuranceId));
            }

            ///Check for Client locations.
            if (!string.IsNullOrEmpty(query.ClientLocationIds))//AA-120
            {
                Criteria = Criteria.And(c => locationIds.Contains(c.ClaimStatusBatchClaim.ClientLocationId.Value));
            }

            ///Check for client providers.
            if (!string.IsNullOrEmpty(query.ClientProviderIds))//AA-120
            {
                Criteria = Criteria.And(c => providerIds.Contains(c.ClaimStatusBatchClaim.ClientProviderId.Value));
            }

            if (exceptionReasonIds.Any())//AA-120
            {
                Criteria = Criteria.And(c => c.ClaimStatusExceptionReasonCategoryId >= 0);
                Criteria = Criteria.And(c => c.ClaimStatusExceptionReasonCategoryId != null && exceptionReasonIds.Contains((ClaimStatusExceptionReasonCategoryEnum)c.ClaimStatusExceptionReasonCategoryId));
            }

            if (!string.IsNullOrEmpty(query.AuthTypeIds))//AA-120
            {
                Criteria = Criteria.And(c => serviceTypeIds.Contains(c.ClaimStatusBatchClaim.ClaimStatusBatch.AuthTypeId ?? 0));
            }

            if (!string.IsNullOrWhiteSpace(query.ProcedureCodes))//AA-120
            {
                Criteria = Criteria.And(c => procedureCodes.Contains(c.ClaimStatusBatchClaim.ProcedureCode.Trim()));
            }
        }
    }

}
