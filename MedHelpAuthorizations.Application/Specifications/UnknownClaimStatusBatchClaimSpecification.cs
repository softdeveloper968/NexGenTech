using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    /// <summary>
    /// EN-37
    /// Represents a specification used to filter and query instances of the ClaimStatusBatchClaim entity with specific criteria.
    /// This specification is designed to filter ClaimStatusBatchClaim objects based on the ClaimLineItemStatusId, specifically targeting those with a ClaimLineItemStatus of "Unknown."
    /// </summary>
    public class UnknownClaimStatusBatchClaimSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public UnknownClaimStatusBatchClaimSpecification()
        {
            Includes.Add(x => x.ClaimStatusTransaction);

            Criteria = bc => true;
            Criteria = Criteria.And(bc => bc.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue
                                          && bc.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Unknown);
        }
    }
}
