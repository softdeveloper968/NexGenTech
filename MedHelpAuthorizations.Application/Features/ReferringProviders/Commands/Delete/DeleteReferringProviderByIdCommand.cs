using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ReferringProviders.Commands.Delete
{
    public class DeleteReferringProviderByIdCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteReferringProviderByIdCommandHandler : IRequestHandler<DeleteReferringProviderByIdCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteReferringProviderByIdCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteReferringProviderByIdCommand command, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.Repository<ReferringProvider>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<ReferringProvider>().DeleteAsync(item);

            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(item.Id, "Referring Provider Deleted");
        }
    }
}
