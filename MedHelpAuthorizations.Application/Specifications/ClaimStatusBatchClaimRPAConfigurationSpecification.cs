using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchClaimRPAConfigurationSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public ClaimStatusBatchClaimRPAConfigurationSpecification(int? rpaInsuranceId, int? authTypeId, int? clientLocationId)
        {
            Includes.Add(x => x.ClientInsurance.RpaInsurance);
            Includes.Add(x => x.ClaimStatusBatch);
            Criteria = bc => true;

            Criteria = Criteria.And(bc => !rpaInsuranceId.HasValue || rpaInsuranceId.Value == 0 || bc.ClientInsurance.RpaInsuranceId == rpaInsuranceId);
            Criteria = Criteria.And(bc => !authTypeId.HasValue || authTypeId.Value == 0 || bc.ClaimStatusBatch.AuthTypeId == authTypeId);
            Criteria = Criteria.And(bc => !clientLocationId.HasValue || clientLocationId.Value == 0 || bc.ClientLocationId == clientLocationId);
        }
    }
}
