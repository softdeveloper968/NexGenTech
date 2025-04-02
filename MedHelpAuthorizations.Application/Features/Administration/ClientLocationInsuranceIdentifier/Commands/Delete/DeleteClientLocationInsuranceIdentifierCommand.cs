using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Commands.Delete
{
    public class DeleteClientLocationInsuranceIdentifierCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientLocationInsuranceIdentifierCommandHandler : IRequestHandler<DeleteClientLocationInsuranceIdentifierCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientLocationInsuranceIdentifierCommandHandler> _localizer;

        public DeleteClientLocationInsuranceIdentifierCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientLocationInsuranceIdentifierCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteClientLocationInsuranceIdentifierCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var locationInsuranceIdentifier = await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>().GetByIdAsync(command.Id);
                await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>().DeleteAsync(locationInsuranceIdentifier);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(locationInsuranceIdentifier.Id, _localizer["ClientLocationInsuranceIdentifier Deleted"]);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}