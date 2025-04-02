using System;
using System.Linq;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public partial class ClaimStatusBatch
    {
        public DateTime GetLastBatchTransactionDate(ClaimStatusBatch claimStatusBatch)
        {
            if (claimStatusBatch.ClaimStatusBatchClaims.Any())
            {
                return claimStatusBatch.ClaimStatusBatchClaims
                    .Where(bc => bc.ClaimStatusTransaction != null)
                    .Select(bc => bc.ClaimStatusTransaction)
                    .MaxBy(t => t.ClaimStatusTransactionEndDateTimeUtc)
                    .ClaimStatusTransactionEndDateTimeUtc;
            }

            return DateTime.MinValue;
        }
    } 
}
