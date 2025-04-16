using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update
{
    public class UpdateIsCredentialInUseCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }       
        public bool IsCredentialInUse { get; set; } = false;
        //public string TargetUrl { get; set; }
        //public string Username { get; set; }
        
        public class UpdateIsCredentialInUseCommandHandler : IRequestHandler<UpdateIsCredentialInUseCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            //private readonly IClientInsuranceRpaConfigurationRepository _ClientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public UpdateIsCredentialInUseCommandHandler(IClientInsuranceRpaConfigurationRepository ClientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
            {

                //_ClientInsuranceRpaConfigurationRepository = ClientInsuranceRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdateIsCredentialInUseCommand command, CancellationToken cancellationToken)
            {

                var clientRpaCredentialConfiguration = await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().Entities.FirstOrDefaultAsync(x => x.Id == command.Id);

                if (clientRpaCredentialConfiguration == null)
                {
                    return Result<int>.Fail($"ClientRpaCredentialConfiguration Not Found.");
                }
                else
                {
                    // Decided not to use the AutoMapper. 
                    // Decided to only aloow update of certain columns
                    clientRpaCredentialConfiguration.IsCredentialInUse = command.IsCredentialInUse;   //AA-23             
                    
                    await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().UpdateAsync(clientRpaCredentialConfiguration);
                    await _unitOfWork.Commit(cancellationToken);

                    return Result<int>.Success(clientRpaCredentialConfiguration.Id, $"Updated ClientRpaCredentialConfiguration Id: {clientRpaCredentialConfiguration.Id}");
                }
            }
        }
    }
}
