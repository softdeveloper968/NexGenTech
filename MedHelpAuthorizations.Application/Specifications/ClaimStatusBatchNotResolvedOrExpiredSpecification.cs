using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusBatchNotDeletedResolvedOrExpiredSpecification : HeroSpecification<ClaimStatusBatch>
    {
        public ClaimStatusBatchNotDeletedResolvedOrExpiredSpecification()
        {
            Criteria = b => true;
            Criteria = Criteria.And(b => !b.AllClaimStatusesResolvedOrExpired && !b.IsDeleted);            
        }
    }
}
