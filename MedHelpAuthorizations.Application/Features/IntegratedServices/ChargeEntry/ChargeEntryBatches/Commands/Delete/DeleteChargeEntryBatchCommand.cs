using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Commands.Delete
{
    public class DeleteChargeEntryBatchCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteTransactionBatchCommandHandler : IRequestHandler<DeleteChargeEntryBatchCommand, Result<int>>
        {
            private readonly IChargeEntryBatchRepository _TransactionBatchRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteTransactionBatchCommandHandler(IChargeEntryBatchRepository TransactionBatchRepository, IUnitOfWork<int> unitOfWork)
            {
                _TransactionBatchRepository = TransactionBatchRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteChargeEntryBatchCommand command, CancellationToken cancellationToken)
            {
                var transactionBatch = await _TransactionBatchRepository.GetByIdAsync(command.Id);

                // Do we want to do a soft delete?? 
                //if (transactionBatch != null)
                //        transactionBatch.IsDeleted = true;
                await _TransactionBatchRepository.DeleteAsync(transactionBatch);

                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(transactionBatch.Id);
            }
        }
    }
}
