using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Create
{
    public class CreateClientInsuranceRpaConfigurationCommand : IRequest<Result<int>>
    {
        public int ClientInsuranceId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }
        public int? AuthTypeId { get; set; }
        public string ExternalId { get; set; }
        public int ClientRpaCredentialConfigurationId { get; set; } //AA-23
        public int? AlternateClientRpaCredentialConfigurationId { get; set; }
        public int? ClientLocationId { get; set; }
        //public List<ClientLocationInsuranceRpaConfigurationDto> ClientLocationInsuranceRpaConfigurations { get; set; } = new();
    }

    public class CreateClientInsuranceRpaConfigurationCommandHandler : IRequestHandler<CreateClientInsuranceRpaConfigurationCommand, Result<int>>
    {
        private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
        private readonly IMapper _mapper;

        private IUnitOfWork<int> _unitOfWork { get; set; }

        public CreateClientInsuranceRpaConfigurationCommandHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateClientInsuranceRpaConfigurationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.AuthTypeId = request.AuthTypeId != 0 ? request.AuthTypeId : null;
                var clientInsuranceRpaConfigExists = await _unitOfWork.Repository<ClientInsuranceRpaConfiguration>().Entities
                                                                        .FirstOrDefaultAsync(circ => circ.ClientInsuranceId == request.ClientInsuranceId
                                                                        && circ.TransactionTypeId == request.TransactionTypeId
                                                                        && circ.AuthTypeId == request.AuthTypeId
                                                                        && circ.ClientLocationId == request.ClientLocationId
                                                                        && !circ.IsDeleted);

                //Prevent Creation an ClientInuranceRpaConfiguration record for same ClientInsuranceId, TransactionType, and AuthType
                if (clientInsuranceRpaConfigExists != null)
                {
                    return await Result<int>.FailAsync("Configuration already exists!");
                }

                var clientInsuranceRpaConfiguration = _mapper.Map<ClientInsuranceRpaConfiguration>(request);
                clientInsuranceRpaConfiguration.AuthTypeId = clientInsuranceRpaConfiguration.AuthTypeId != 0 ? clientInsuranceRpaConfiguration.AuthTypeId : null;
                clientInsuranceRpaConfiguration.ClientLocationId = clientInsuranceRpaConfiguration.ClientLocationId != 0 ? clientInsuranceRpaConfiguration.ClientLocationId : null;
                await _clientInsuranceRpaConfigurationRepository.InsertAsync(clientInsuranceRpaConfiguration);

                if (clientInsuranceRpaConfiguration.ClientLocationId == null)
                {
                    var updateClientInsurance = await _unitOfWork.Repository<ClientInsurance>().GetByIdAsync(request.ClientInsuranceId);
                    if (updateClientInsurance != null && !updateClientInsurance.RequireLocationInput)
                    {
                        updateClientInsurance.RequireLocationInput = true;
                    }
                    await _unitOfWork.Repository<ClientInsurance>().UpdateAsync(updateClientInsurance);
                    await _unitOfWork.Commit(cancellationToken);
                }
               
                return await Result<int>.SuccessAsync(clientInsuranceRpaConfiguration.Id, "Configuration sucessfully Addded");
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
    }
}
