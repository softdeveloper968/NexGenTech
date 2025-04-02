using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Commands.Delete
{
    public class DeleteChargeEntryRpaConfigurationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteChargeEntryRpaConfigurationCommandHandler : IRequestHandler<DeleteChargeEntryRpaConfigurationCommand, Result<int>>
        {
            private readonly IChargeEntryRpaConfigurationRepository _ChargeEntryRpaConfigurationRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteChargeEntryRpaConfigurationCommandHandler(IChargeEntryRpaConfigurationRepository ChargeEntryRpaConfigurationRepository, IUnitOfWork<int> unitOfWork)
            {
                _ChargeEntryRpaConfigurationRepository = ChargeEntryRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteChargeEntryRpaConfigurationCommand command, CancellationToken cancellationToken)
            {
                var chargeEntryRpaConfiguration = await _ChargeEntryRpaConfigurationRepository.GetByIdAsync(command.Id);
                await _ChargeEntryRpaConfigurationRepository.DeleteAsync(chargeEntryRpaConfiguration);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(chargeEntryRpaConfiguration.Id);
            }
        }
    }
}
