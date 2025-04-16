using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchClaimUnMappedToFeeScheduleSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public ClaimStatusBatchClaimUnMappedToFeeScheduleSpecification()
        {
            Criteria = bc => true;           
            Criteria = Criteria.And(c => c.ClientFeeScheduleEntryId == null);
        }
    }
}
