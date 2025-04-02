using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Command.Delete
{
    public class DeleteClientDocumentCommand : IRequest<Result<int>> //EN-791
    {
        public int Id { get; set; }
    }

    internal class DeleteClientDocumentCommandHandler : IRequestHandler<DeleteClientDocumentCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientDocumentCommandHandler> _localizer;

        public DeleteClientDocumentCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientDocumentCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientDocumentCommand command, CancellationToken cancellationToken)
        {
            var clientDocument = await _unitOfWork.Repository<Domain.Entities.ClientDocument>().GetByIdAsync(command.Id);
            if (clientDocument == null)
            {
                return await Result<int>.FailAsync(_localizer["ClientDocument Not Found"]);
            }

            await _unitOfWork.Repository<Domain.Entities.ClientDocument>().DeleteAsync(clientDocument);
            await _unitOfWork.Commit(cancellationToken);

            return await Result<int>.SuccessAsync(clientDocument.Id, _localizer["ClientDocument Deleted"]);
        }
    }

}
