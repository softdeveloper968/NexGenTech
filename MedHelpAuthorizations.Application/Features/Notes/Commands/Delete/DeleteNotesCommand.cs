using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.Notes.Commands.Delete
{
    public class DeleteNoteCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, Result<int>>
    {        
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteNoteCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public async Task<Result<int>> Handle(DeleteNoteCommand command, CancellationToken cancellationToken)
        {   
            var Note = await _unitOfWork.Repository<Note>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<Note>().DeleteAsync(Note);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(Note.Id, "Note Deleted");        }
    }
}