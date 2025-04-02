using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusAttemptsQualificationFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {

        public ClaimStatusAttemptsQualificationFilterSpecification()
        {

            Includes.Add(bc => bc.ClaimStatusTransaction);
            Includes.Add(bc => bc.ClaimStatusTransaction.ClaimLineItemStatus);
            Includes.Add(bc => bc.ClaimStatusTransaction.ClaimStatusTransactionHistories);

            Criteria = bc => true;

            Criteria = Criteria.And(bc => bc.ClaimStatusTransaction != null
                         ? bc.ClaimStatusTransaction.ClaimStatusTransactionHistories.Count(hx => hx.CreatedOn > DateTime.UtcNow.AddMonths(-3)
                             && hx.ClaimLineItemStatusId == bc.ClaimStatusTransaction.ClaimLineItemStatusId) < bc.ClaimStatusTransaction.ClaimLineItemStatus.MaximumResolutionAttempts : true);
        }
    }
}
