using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Command.Delete
{
    public class DeleteClientNoteCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
    public class DeleteClientNoteCommandHandler : IRequestHandler<DeleteClientNoteCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientNoteCommandHandler> _localizer;
        public DeleteClientNoteCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientNoteCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }
        public async Task<Result<int>> Handle(DeleteClientNoteCommand command, CancellationToken cancellationToken)
        {
            var clientNote = await _unitOfWork.Repository<ClientNote>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<ClientNote>().DeleteAsync(clientNote);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(clientNote.Id, _localizer["ClientNote Deleted"]);
        }
    }
}
