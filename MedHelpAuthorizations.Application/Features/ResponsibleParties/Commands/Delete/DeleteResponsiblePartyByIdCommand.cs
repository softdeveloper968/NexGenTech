using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ResponsibleParties.Commands.Delete
{
    public class DeleteResponsiblePartyByIdCommand : IRequest<Result<int>>    
    {
        public int Id { get; set; }
    }

    public class DeleteResponsiblePartyByIdCommandHandler : IRequestHandler<DeleteResponsiblePartyByIdCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteResponsiblePartyByIdCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(DeleteResponsiblePartyByIdCommand command, CancellationToken cancellationToken)
        {
            var item = await _unitOfWork.Repository<ResponsibleParty>().GetByIdAsync(command.Id);            
            await _unitOfWork.Repository<ResponsibleParty>().DeleteAsync(item);
            await _unitOfWork.Commit(cancellationToken);

            return await Result<int>.SuccessAsync(item.Id, "ResponsibleParty Deleted");
        }
    }
}
