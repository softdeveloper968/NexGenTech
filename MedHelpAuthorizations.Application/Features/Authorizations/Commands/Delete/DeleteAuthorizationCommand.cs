using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Features.Authorizations.Commands.Delete
{
    public class DeleteAuthorizationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteAuthorizationCommandHandler : IRequestHandler<DeleteAuthorizationCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteAuthorizationCommandHandler> _localizer;

        public DeleteAuthorizationCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteAuthorizationCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteAuthorizationCommand command, CancellationToken cancellationToken)
        {
            var authorization = await _unitOfWork.Repository<Authorization>().Entities
                .Include(x => x.InitialAuthorizations)
                .Include(x => x.SucceededAuthorizations)
                .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            //var notes = authorization.Notes;
            if (authorization != null)
            {
                //delete associated ConcurrentAuthorization for InitialAuthorization Records
                foreach (var ia in authorization.InitialAuthorizations)
                {
                    await _unitOfWork.Repository<ConcurrentAuthorization>().DeleteAsync(ia);
                }

                //delete associated ConcurrentAuthorization for SucceededAuthorization Records
                foreach (var sa in authorization.SucceededAuthorizations)
                {
                    await _unitOfWork.Repository<ConcurrentAuthorization>().DeleteAsync(sa);
                }

                //delete associated notes
                //foreach (var nt in notes)
                //{
                //    await _unitOfWork.Repository<Note>().DeleteAsync(nt);
                //}
            }

            await _unitOfWork.Repository<Authorization>().DeleteAsync(authorization);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(authorization.Id, _localizer["Authorization Deleted"]);
        }
    }
}