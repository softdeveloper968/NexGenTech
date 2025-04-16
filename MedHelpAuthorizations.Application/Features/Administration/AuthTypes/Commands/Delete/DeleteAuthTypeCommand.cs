using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Commands.Delete
{
    public class DeleteAuthTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteAuthTypeCommandHandler : IRequestHandler<DeleteAuthTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteAuthTypeCommandHandler> _localizer;

        public DeleteAuthTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteAuthTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteAuthTypeCommand command, CancellationToken cancellationToken)
        {
            var authorization = await _unitOfWork.Repository<AuthType>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<AuthType>().DeleteAsync(authorization);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(authorization.Id, _localizer["AuthType Deleted"]);
        }
    }
}