using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientPlacesOfService.Commands.Delete
{
    public class DeleteClientPlaceOfServiceCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientPlaceOfServiceCommandHandler : IRequestHandler<DeleteClientPlaceOfServiceCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientPlaceOfServiceCommandHandler> _localizer;

        public DeleteClientPlaceOfServiceCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientPlaceOfServiceCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientPlaceOfServiceCommand command, CancellationToken cancellationToken)
        {
            var clientPlaceOfService = await _unitOfWork.Repository<Domain.Entities.ClientPlaceOfService>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<Domain.Entities.ClientPlaceOfService>().DeleteAsync(clientPlaceOfService);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(clientPlaceOfService.Id, _localizer["ClientPlaceOfService Deleted"]);
        }
    }
}
