using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusDaysWaitLapsedFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {

        public ClaimStatusDaysWaitLapsedFilterSpecification()
        {
            Criteria = bc => true;

            Includes.Add(bc => bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatus);

            Criteria = Criteria.And(bc => (bc.ClaimStatusTransaction != null && bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId != null)
            && (bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.HasValue
                    ? bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.Value.AddDays(bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatus.DaysWaitBetweenAttempts) < DateTime.UtcNow.AddDays(1)
                    : bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.CreatedOn.AddDays(bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.UpdatedClaimLineItemStatus.DaysWaitBetweenAttempts) < DateTime.UtcNow.AddDays(1)));
        }
    }
}
