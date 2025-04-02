using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Delete
{
    public class DeleteClientInsuranceRpaConfigurationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteClientInsuranceRpaConfigurationCommandHandler : IRequestHandler<DeleteClientInsuranceRpaConfigurationCommand, Result<int>>
        {
            private readonly IClientInsuranceRpaConfigurationRepository _ClientInsuranceRpaConfigurationRepository;
            private readonly IUnitOfWork<int> _unitOfWork;

            public DeleteClientInsuranceRpaConfigurationCommandHandler(IClientInsuranceRpaConfigurationRepository ClientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork)
            {
                _ClientInsuranceRpaConfigurationRepository = ClientInsuranceRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<int>> Handle(DeleteClientInsuranceRpaConfigurationCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var clientInsuranceRpaConfiguration = await _ClientInsuranceRpaConfigurationRepository.GetByIdAsync(command.Id);
                    await _ClientInsuranceRpaConfigurationRepository.DeleteAsync(clientInsuranceRpaConfiguration);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(clientInsuranceRpaConfiguration.Id, "Configuration sucessfullt Deleted");
                }
                catch (System.Exception ex)
                {

                    throw;
                }
                
            }
        }
    }
}
