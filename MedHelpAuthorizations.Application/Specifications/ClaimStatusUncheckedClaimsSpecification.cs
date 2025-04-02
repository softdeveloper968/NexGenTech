using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusUncheckedClaimsSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public ClaimStatusUncheckedClaimsSpecification()
        {
            Includes.Add(x => x.ClaimStatusBatch.ClientInsurance.RpaInsurance);
            Includes.Add(x => x.ClaimStatusBatch);
            Includes.Add(x => x.ClientLocation);
            Criteria = bc => true;

            //Batch is not deleted
            Criteria = Criteria.And(bc => !bc.ClaimStatusBatch.IsDeleted);

            // Claim is not marked as deleted
            Criteria = Criteria.And(bc => !bc.IsDeleted);

            //Claim is not Supplanted
            Criteria = Criteria.And(bc => !bc.IsSupplanted);

            //Claim does not have a transaction
            Criteria = Criteria.And(bc => !bc.ClaimStatusTransactionId.HasValue);

            //And claimbilledon date has to be 1 day older than ClaimBilledOnWaitDays
            Criteria = Criteria.And(bc => bc.ClaimBilledOn.Value.Date.AddDays(bc.ClientInsurance.RpaInsurance.ClaimBilledOnWaitDays + 1) <= DateTime.UtcNow.Date);
        }
    }
}
