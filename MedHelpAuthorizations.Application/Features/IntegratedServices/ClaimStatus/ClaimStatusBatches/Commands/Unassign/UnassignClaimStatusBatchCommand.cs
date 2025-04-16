using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Unassign
{
    public class UnassignClaimStatusBatchCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class UnassignClaimStatusBatchCommandHandler : IRequestHandler<UnassignClaimStatusBatchCommand, Result<int>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;

        public UnassignClaimStatusBatchCommandHandler(IClaimStatusQueryService claimStatusQueryService)
        {
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<int>> Handle(UnassignClaimStatusBatchCommand command, CancellationToken cancellationToken)
        {
            try
            {
                await _claimStatusQueryService.UnassignClaimStatusBatchAsync(command.Id);
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return await Result<int>.FailAsync($"Claim Status Batch Unassignment Failed Id = {command.Id}. Error: {errorMessage}").ConfigureAwait(true);
            }

            return await Result<int>.SuccessAsync(command.Id).ConfigureAwait(true);
        }
    }    
}
