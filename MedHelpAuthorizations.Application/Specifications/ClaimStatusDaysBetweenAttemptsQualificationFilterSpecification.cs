using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusDaysBetweenAttemptsQualificationFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {

        public ClaimStatusDaysBetweenAttemptsQualificationFilterSpecification()
        {
            Criteria = bc => true;

            Includes.Add(bc => bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ);
            Includes.Add(bc => bc.ClaimStatusTransaction.ClaimLineItemStatus);

            Criteria = Criteria.And(bc => (bc.ClaimStatusTransaction == null || bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹId == null)
            || (bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.HasValue
                    ? bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.LastModifiedOn.Value.AddDays(bc.ClaimStatusTransaction.ClaimLineItemStatus.DaysWaitBetweenAttempts) <= DateTime.UtcNow
                    : bc.ClaimStatusTransaction.ClaimStatusTransactionLineItemStatusChangẹ.CreatedOn.AddDays(bc.ClaimStatusTransaction.ClaimLineItemStatus.DaysWaitBetweenAttempts) <= DateTime.UtcNow));
        }
    }
}
