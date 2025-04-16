using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Delete
{
    public class DeleteChargeEntryTransactionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteChargeEntryTransactionCommandHandler : IRequestHandler<DeleteChargeEntryTransactionCommand, Result<int>>
        {
            private readonly IChargeEntryTransactionRepository _ChargeEntryTransactionRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteChargeEntryTransactionCommandHandler(IChargeEntryTransactionRepository ChargeEntryTransactionRepository, IUnitOfWork<int> unitOfWork)
            {
                _ChargeEntryTransactionRepository = ChargeEntryTransactionRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteChargeEntryTransactionCommand command, CancellationToken cancellationToken)
            {
                var claimStatusTransaction = await _ChargeEntryTransactionRepository.GetByIdAsync(command.Id);
                await _ChargeEntryTransactionRepository.DeleteAsync(claimStatusTransaction);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(claimStatusTransaction.Id);
            }
        }
    }
}
