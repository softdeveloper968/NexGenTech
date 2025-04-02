using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Delete
{
    public class DeleteClaimStatusTransactionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteClaimStatusTransactionCommandHandler : IRequestHandler<DeleteClaimStatusTransactionCommand, Result<int>>
        {
            private readonly IClaimStatusTransactionRepository _ClaimStatusTransactionRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteClaimStatusTransactionCommandHandler(IClaimStatusTransactionRepository ClaimStatusTransactionRepository, IUnitOfWork<int> unitOfWork)
            {
                _ClaimStatusTransactionRepository = ClaimStatusTransactionRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteClaimStatusTransactionCommand command, CancellationToken cancellationToken)
            {
                var claimStatusTransaction = await _ClaimStatusTransactionRepository.GetByIdAsync(command.Id);
                await _ClaimStatusTransactionRepository.DeleteAsync(claimStatusTransaction);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(claimStatusTransaction.Id);
            }
        }
    }
}
