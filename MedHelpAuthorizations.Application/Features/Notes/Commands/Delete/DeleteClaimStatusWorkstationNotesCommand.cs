using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Notes.Commands.Delete
{
    public class DeleteClaimStatusWorkstationNotesCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClaimStatusWorkstationNotesCommandHandler : IRequestHandler<DeleteClaimStatusWorkstationNotesCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteClaimStatusWorkstationNotesCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteClaimStatusWorkstationNotesCommand command, CancellationToken cancellationToken)
        {
            var Note = await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().DeleteAsync(Note);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(Note.Id, "Note Deleted");
        }
    }
}
