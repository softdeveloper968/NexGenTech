using System;

namespace MedHelpAuthorizations.Application.Responses.IntegratedServices
{
    public class ClaimStatusBatchLastTransactionResponse
    {
        public int ClaimStatusBatchId { get; set; }
        public DateTime LastClaimStatusBatchTransactionDate { get; set; }
        public string AssignedToRpaLocalProcessIds { get; set; }
    }
}
