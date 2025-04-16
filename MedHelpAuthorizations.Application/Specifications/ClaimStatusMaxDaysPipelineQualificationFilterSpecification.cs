using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System;
using System.Linq;

namespace MedHelpAuthorizations.Application.Specifications
{
    public class ClaimStatusMaxDaysPipelineQualificationFilterSpecification : HeroSpecification<ClaimStatusBatchClaim>
    {

        public ClaimStatusMaxDaysPipelineQualificationFilterSpecification()
        {
            Includes.Add(bc => bc.ClaimStatusTransaction.ClaimLineItemStatus);
            Includes.Add(bc => bc.ClaimStatusTransaction.ClaimStatusTransactionHistories);

            Criteria = bc => true;

            //Only hold for 7 if no transaction can be gottem within 7 days/ TODO: We need to setup services that look for something like this that should not happen.. ever
            // Otherwise.. Hold for given pipeline days given for the transaction claimLineItemStatus value
            Criteria = Criteria.And(bc => bc.ClaimStatusTransactionId == null
                || (bc.ClaimStatusTransaction != null && bc.ClaimStatusTransaction.ClaimLineItemStatus.MaximumPipelineDays > 0 && (bc.ClaimStatusTransaction.LastModifiedOn.HasValue
                    ? bc.ClaimStatusTransaction.LastModifiedOn.Value.AddDays(bc.ClaimStatusTransaction.ClaimLineItemStatus.MaximumPipelineDays) >= DateTime.UtcNow
                    : bc.ClaimStatusTransaction.CreatedOn.AddDays(bc.ClaimStatusTransaction.ClaimLineItemStatus.MaximumPipelineDays) >= DateTime.UtcNow
                || bc.ClaimStatusTransaction.ClaimStatusTransactionHistories
                        .Count(hx => hx.CreatedOn > DateTime.UtcNow.AddMonths(-3) && hx.ClaimLineItemStatusId == bc.ClaimStatusTransaction.ClaimLineItemStatusId)
                            < bc.ClaimStatusTransaction.ClaimLineItemStatus.MinimumResolutionAttempts)));

        }
    }
}
