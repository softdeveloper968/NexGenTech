using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusOmitDeniedPolicyNumberFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {

        public ClaimStatusOmitDeniedPolicyNumberFilterSpecification()
        {
            Includes.Add(x => x.ClaimStatusTransaction);
            Criteria = bc => true;
            
            Criteria = Criteria.And(bc => !(bc.ClaimStatusTransaction.ClaimLineItemStatusId == Domain.Entities.Enums.ClaimLineItemStatusEnum.Denied 
                                            && bc.ClaimStatusTransaction.ClaimStatusExceptionReasonCategoryId == Domain.Entities.Enums.ClaimStatusExceptionReasonCategoryEnum.PolicyNumber));
        }
    }
}
