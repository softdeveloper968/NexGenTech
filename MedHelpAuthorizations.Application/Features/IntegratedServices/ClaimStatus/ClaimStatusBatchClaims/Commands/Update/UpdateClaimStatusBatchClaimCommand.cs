using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Commands.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Commands.Update
{
    public class UpdateClaimStatusBatchClaimCommand : BaseClaimStatusBatchClaimCommand
    {
        public string ProcedureCode { get; set; }
    }
    public class UpdateClaimStatusBatchClaimCommandHandler : IRequestHandler<UpdateClaimStatusBatchClaimCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateClaimStatusBatchClaimCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {           
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(UpdateClaimStatusBatchClaimCommand command, CancellationToken cancellationToken)
        {
            var claimStatusBatchClaim = await _unitOfWork.Repository<ClaimStatusBatchClaim>().GetByIdAsync(command.Id);

            if (claimStatusBatchClaim == null)
            {
                return Result<int>.Fail($"ClaimStatusBatchClaim was not found; update failed.");
            }
            else
            {
                claimStatusBatchClaim.ProcedureCode = command.ProcedureCode ?? claimStatusBatchClaim.ProcedureCode;
                claimStatusBatchClaim.PolicyNumber = command.PolicyNumber ?? claimStatusBatchClaim.PolicyNumber;
                claimStatusBatchClaim.PolicyNumberUpdatedOn = !string.IsNullOrWhiteSpace(command.PolicyNumber) ? DateTime.UtcNow : claimStatusBatchClaim.PolicyNumberUpdatedOn;

				await _unitOfWork.Repository<ClaimStatusBatchClaim>().UpdateAsync(claimStatusBatchClaim);
                await _unitOfWork.Commit(cancellationToken);
                
                return Result<int>.Success(claimStatusBatchClaim.Id);
            }            
        }
    }
}
