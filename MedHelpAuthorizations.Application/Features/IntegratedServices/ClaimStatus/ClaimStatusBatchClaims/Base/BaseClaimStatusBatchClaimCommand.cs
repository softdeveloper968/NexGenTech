using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Commands.Base
{
    public class BaseClaimStatusBatchClaimCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int? ClaimStatusTransactionId { get; set; }
        public string PolicyNumber { get; set; }
    }
}
