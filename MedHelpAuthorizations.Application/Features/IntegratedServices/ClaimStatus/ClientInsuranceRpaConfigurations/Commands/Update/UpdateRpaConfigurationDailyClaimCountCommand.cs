using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update
{
    public class UpdateRpaConfigurationDailyClaimCountCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }       
        public int AddedClaimCount { get; set; }
        
        public class UpdateRpaConfigurationDailyClaimCountCommandHandler : IRequestHandler<UpdateRpaConfigurationDailyClaimCountCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IClientInsuranceRpaConfigurationRepository _ClientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public UpdateRpaConfigurationDailyClaimCountCommandHandler(IClientInsuranceRpaConfigurationRepository ClientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
            {

                _ClientInsuranceRpaConfigurationRepository = ClientInsuranceRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdateRpaConfigurationDailyClaimCountCommand command, CancellationToken cancellationToken)
            {

                var ClientInsuranceRpaConfiguration = await _ClientInsuranceRpaConfigurationRepository.GetByIdAsync(command.Id);

                if (ClientInsuranceRpaConfiguration == null)
                {
                    return Result<int>.Fail($"ClientInsuranceRpaConfiguration Not Found.");
                }
                else
                {
                    // Decided not to use the AutoMapper. 
                    // Decided to only allow update of certain columns
                    ClientInsuranceRpaConfiguration.CurrentDayClaimCount = ClientInsuranceRpaConfiguration.CurrentDayClaimCount + command.AddedClaimCount;                
                    
                    await _ClientInsuranceRpaConfigurationRepository.UpdateAsync(ClientInsuranceRpaConfiguration);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(ClientInsuranceRpaConfiguration.Id);
                }
            }
        }
    }
}
