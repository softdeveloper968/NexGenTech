using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update
{
    public class UpdateRpaConfigurationExpiryWarningCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }       
        public bool ExpiryWarningReported { get; set; } = false;
        
        public class UpdateRpaConfigurationExpiryWarningCommandHandler : IRequestHandler<UpdateRpaConfigurationExpiryWarningCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IClientInsuranceRpaConfigurationRepository _ClientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public UpdateRpaConfigurationExpiryWarningCommandHandler(IClientInsuranceRpaConfigurationRepository ClientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
            {

                _ClientInsuranceRpaConfigurationRepository = ClientInsuranceRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdateRpaConfigurationExpiryWarningCommand command, CancellationToken cancellationToken)
            {
                var ClientInsuranceRpaConfiguration = await _ClientInsuranceRpaConfigurationRepository.GetByIdAsync(command.Id);

                if (ClientInsuranceRpaConfiguration == null)
                {
                    return Result<int>.Fail($"ClientInsuranceRpaConfiguration Not Found.");
                }
                else
                {
                    // Decided not to use the AutoMapper. 
                    // Decided to only aloow update of certain columns
                    ClientInsuranceRpaConfiguration.ClientRpaCredentialConfiguration.ExpiryWarningReported = command.ExpiryWarningReported;  //AA-23               
                    
                    await _ClientInsuranceRpaConfigurationRepository.UpdateAsync(ClientInsuranceRpaConfiguration);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(ClientInsuranceRpaConfiguration.Id);
                }
            }
        }
    }
}
