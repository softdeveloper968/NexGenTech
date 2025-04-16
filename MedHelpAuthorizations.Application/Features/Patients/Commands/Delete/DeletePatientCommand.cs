using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Patients.Commands.Delete
{
    public class DeletePatientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeletePatientCommandHandler> _localizer;

        public DeletePatientCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePatientCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Repository<Patient>().GetByIdAsync(command.Id);
            await _unitOfWork.Repository<Patient>().DeleteAsync(patient);
            await _unitOfWork.Commit(cancellationToken);
            return await Result<int>.SuccessAsync(patient.Id, _localizer["Patient Deleted"]);
        }
    }
}