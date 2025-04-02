using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Commands.Delete
{
    public class DeleteClientEncounterTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientEncounterTypeCommandHandler : IRequestHandler<DeleteClientEncounterTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientEncounterTypeCommandHandler> _localizer;

        public DeleteClientEncounterTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientEncounterTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientEncounterTypeCommand command, CancellationToken cancellationToken)
        {
            var encounterType = await _unitOfWork.Repository<ClientEncounterType>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<ClientEncounterType>().DeleteAsync(encounterType);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(encounterType.Id, _localizer["EncounterType Deleted"]);
        }
    }
}