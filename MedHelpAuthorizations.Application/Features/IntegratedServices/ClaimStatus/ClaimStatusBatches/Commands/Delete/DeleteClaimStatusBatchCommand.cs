using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Commands.Delete
{
    public class DeleteClaimStatusBatchCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteTransactionBatchCommandHandler : IRequestHandler<DeleteClaimStatusBatchCommand, Result<int>>
        {
            private readonly IClaimStatusBatchRepository _TransactionBatchRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteTransactionBatchCommandHandler(IClaimStatusBatchRepository TransactionBatchRepository, IUnitOfWork<int> unitOfWork)
            {
                _TransactionBatchRepository = TransactionBatchRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteClaimStatusBatchCommand command, CancellationToken cancellationToken)
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
