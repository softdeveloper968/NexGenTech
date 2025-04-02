using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Commands.Delete
{
    public class DeleteApiKeyCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteApiKeyCommandHandler : IRequestHandler<DeleteApiKeyCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteApiKeyCommandHandler> _localizer;

        public DeleteApiKeyCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteApiKeyCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteApiKeyCommand command, CancellationToken cancellationToken)
        {
            var authorization = await _unitOfWork.Repository<ClientApiIntegrationKey>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<ClientApiIntegrationKey>().DeleteAsync(authorization);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(authorization.Id, _localizer["Client ApiKey Deleted"]);
        }
    }
}
