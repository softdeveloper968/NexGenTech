using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update
{
    public class UpdateClientInsuranceRpaConfigurationFailureCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public bool FailureReported { get; set; } = false;
        public string FailureMessage { get; set; } = string.Empty;

        public class UpdateClientInsuranceRpaConfigurationFailureCommandHandler : IRequestHandler<UpdateClientInsuranceRpaConfigurationFailureCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IClientInsuranceRpaConfigurationRepository _ClientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public UpdateClientInsuranceRpaConfigurationFailureCommandHandler(IClientInsuranceRpaConfigurationRepository ClientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
            {

                _ClientInsuranceRpaConfigurationRepository = ClientInsuranceRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdateClientInsuranceRpaConfigurationFailureCommand command, CancellationToken cancellationToken)
            {
                var ClientInsuranceRpaConfiguration = await _ClientInsuranceRpaConfigurationRepository.GetByIdAsync(command.Id);

                if (ClientInsuranceRpaConfiguration == null)
                {
                    return Result<int>.Fail($"ClientInsuranceRpaConfiguration Not Found.");
                }
                else
                {
                    ClientInsuranceRpaConfiguration.ClientRpaCredentialConfiguration.FailureReported = command.FailureReported;
                    ClientInsuranceRpaConfiguration.ClientRpaCredentialConfiguration.FailureMessage = command.FailureMessage;

                    await _ClientInsuranceRpaConfigurationRepository.UpdateAsync(ClientInsuranceRpaConfiguration);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(ClientInsuranceRpaConfiguration.Id);
                }
            }
        }
    }
}
