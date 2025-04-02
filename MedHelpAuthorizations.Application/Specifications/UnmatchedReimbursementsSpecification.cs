using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Extensions;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class UnmatchedReimbursementsSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {
        public UnmatchedReimbursementsSpecification(int clientId)
        {
            Includes.Add(x => x.ClientFeeScheduleEntry);
            Includes.Add(x => x.ClientFeeScheduleEntry.ClientFeeSchedule);
            Includes.Add(x => x.ClaimStatusTransaction);
            Includes.Add(x => x.ClientInsurance);

            var oneYearAgo = DateTime.Now.GetLastYearValueForFeeSchedule();

            Criteria = p => true;

            // Criteria for identifying unmatched reimbursements
            Criteria = Criteria.And(batchClaim =>
               batchClaim.ClientId == clientId &&
               batchClaim.CreatedOn > oneYearAgo &&
               batchClaim.ClientFeeScheduleEntry != null &&
               batchClaim.ClientFeeScheduleEntry.IsReimbursable == false &&
               batchClaim.ClaimStatusTransaction != null &&
               batchClaim.ClaimStatusTransaction.TotalAllowedAmount > 0.0m);
        }
    }
}
