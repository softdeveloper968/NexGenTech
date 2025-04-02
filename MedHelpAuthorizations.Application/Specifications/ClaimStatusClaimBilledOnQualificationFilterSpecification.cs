using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusClaimBilledOnQualificationFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {

        public ClaimStatusClaimBilledOnQualificationFilterSpecification()
        {
            Includes.Add(x => x.ClaimStatusBatch.ClientInsurance.RpaInsurance);
            Criteria = bc => true;
            
            //Has to be billed more than 3 days ago if ClaimBilledDateProvided 
            Criteria = Criteria.And(bc => bc.ClaimBilledOn.Value.Date.AddDays(bc.ClaimStatusBatch.ClientInsurance.RpaInsurance.ClaimBilledOnWaitDays) <= DateTime.UtcNow);
        }
    }
}
