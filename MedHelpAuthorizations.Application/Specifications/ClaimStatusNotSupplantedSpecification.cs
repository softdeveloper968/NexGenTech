using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchClaimNotSupplantedSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public ClaimStatusBatchClaimNotSupplantedSpecification()
        {
            Criteria = bc => true;

            Criteria = Criteria.And(c => !c.IsSupplanted);
            Criteria = Criteria.And(c => !c.IsDeleted);
        }
        
    }
}
