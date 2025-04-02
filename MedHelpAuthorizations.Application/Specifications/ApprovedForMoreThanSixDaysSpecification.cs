using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ApprovedForMoreThanSixDaysSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public ApprovedForMoreThanSixDaysSpecification()
        {
            Includes.Add(x => x.ClaimStatusTransaction);

            Criteria = bc => true;
            Criteria = Criteria.And(bc => bc.ClaimStatusTransaction.ClaimLineItemStatusId.HasValue 
                                          && bc.ClaimStatusTransaction.ClaimLineItemStatusId == ClaimLineItemStatusEnum.Approved);

            //Criteria = Criteria.And(bc => bc.ClaimStatusTransaction.LastModifiedOn.HasValue
            //                              && bc.ClaimStatusTransaction.LastModifiedOn.Value.Date.AddDays(6) <= DateTime.UtcNow.Date);
        }
    }
}
