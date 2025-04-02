using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Commands.Delete
{
    public class DeleteClientCptCodeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientCptCodeCommandHandler : IRequestHandler<DeleteClientCptCodeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientCptCodeCommandHandler> _localizer;

        public DeleteClientCptCodeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientCptCodeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientCptCodeCommand command, CancellationToken cancellationToken)
        {
            var clientCptCode = await _unitOfWork.Repository<ClientCptCode>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<ClientCptCode>().DeleteAsync(clientCptCode);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(clientCptCode.Id, _localizer["ClientCptCode Deleted"]);
        }
    }
}