using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.UserAlerts.Commands.Delete
{
    public class DeleteUserAlertCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteUserAlertCommandHandler : IRequestHandler<DeleteUserAlertCommand, Result<int>>
    {        
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteUserAlertCommandHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }

        public async Task<Result<int>> Handle(DeleteUserAlertCommand command, CancellationToken cancellationToken)
        {   
            var UserAlert = await _unitOfWork.Repository<UserAlert>().GetByIdAsync(command.Id);
            UserAlert.IsRemoved = true;
            await _unitOfWork.Repository<UserAlert>().UpdateAsync(UserAlert);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(UserAlert.Id, "UserAlert Deleted");        }
    }
}