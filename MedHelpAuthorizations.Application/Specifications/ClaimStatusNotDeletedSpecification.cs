using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchClaimNotDeletedSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public ClaimStatusBatchClaimNotDeletedSpecification()
        {
            Criteria = bc => true;

            Criteria = Criteria.And(c => !c.IsDeleted);
            Criteria = Criteria.And(c => !c.ClaimStatusBatch.IsDeleted);
        }
    }
}
