using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Base;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetByBatchId
{
    public class GetClaimStatusBatchClaimsByBatchIdResponse : GetClaimStatusBatchClaimBaseResponse
    {
        public ClaimLineItemStatusEnum? CurrentLineItemStatusId { get; set; }

        public string CurrentExceptionReason { get; set; }

        public string EligibilityPolicyNumber { get; set; }
        //public ClientLocationInsuranceIdentifierDto ClientLocationInsuranceIdentifierDto { get; set; }
        public string ClientLocationInsuranceIdentifierString { get; set; }

        public string PayerIdentifier { get; set; }
    }
}
