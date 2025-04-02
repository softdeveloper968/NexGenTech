using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Commands.Update
{
    public class UpdateClientInsuranceRpaConfigurationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public bool FailureReported { get; set; } = false;
        public string FailureMessage { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string ExternalId { get; set; }
        public int? AuthTypeId { get; set; }
        public int ClientRpaCredentialConfigurationId { get; set; } //AA-23
        public int? AlternateClientRpaCredentialConfigurationId { get; set; }
        public int? ClientLocationId { get; set; }


        public class UpdateClientInsuranceRpaConfigurationCommandHandler : IRequestHandler<UpdateClientInsuranceRpaConfigurationCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IClientInsuranceRpaConfigurationRepository _ClientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public UpdateClientInsuranceRpaConfigurationCommandHandler(IClientInsuranceRpaConfigurationRepository ClientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
            {

                _ClientInsuranceRpaConfigurationRepository = ClientInsuranceRpaConfigurationRepository;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<int>> Handle(UpdateClientInsuranceRpaConfigurationCommand command, CancellationToken cancellationToken) //Updated AA-23
            {
                ClientInsuranceRpaConfiguration existingClientInsuranceRpaConfiguration = await _ClientInsuranceRpaConfigurationRepository.GetByIdAsync(command.Id);

                if (existingClientInsuranceRpaConfiguration == null)
                {
                    return Result<int>.Fail($"ClientInsuranceRpaConfiguration Not Found.");
                }

                if(command.AlternateClientRpaCredentialConfigurationId == 0)
                {
                    command.AlternateClientRpaCredentialConfigurationId = null;
                }

                // Check for existing ClientInsuranceRpaConfiguration with the same ClientInsuranceId, TransactionTypeId, AuthTypeId, and ClientLocationId
                ClientInsuranceRpaConfiguration clientInsuranceRpaConfigExists = await _unitOfWork.Repository<ClientInsuranceRpaConfiguration>().Entities
                    .Where(circ => circ.ClientInsuranceId == existingClientInsuranceRpaConfiguration.ClientInsuranceId
                                && circ.TransactionTypeId == existingClientInsuranceRpaConfiguration.TransactionTypeId
                                && circ.AuthTypeId == command.AuthTypeId
                                && circ.ClientLocationId == command.ClientLocationId
                                && circ.Id != command.Id // Exclude the current record being updated
                                && !circ.IsDeleted)
                               .FirstOrDefaultAsync();

                // Prevent updating ClientInsuranceRpaConfiguration with the same ClientInsuranceId, TransactionType, and AuthType
                if (clientInsuranceRpaConfigExists != null)
                {
                    return await Result<int>.FailAsync("Configuration already exists!");
                }


                try
                {
                    existingClientInsuranceRpaConfiguration.IsDeleted = command.IsDeleted ?? existingClientInsuranceRpaConfiguration.IsDeleted;
                    //ClientInsuranceRpaConfiguration.ClientInsuranceId = command. ?? ClientInsuranceRpaConfiguration.InsuranceId;
                    existingClientInsuranceRpaConfiguration.AuthTypeId = command.AuthTypeId == 0 ? null : command.AuthTypeId;
                    //ClientInsuranceRpaConfiguration.TransactionTypeId = command.TransactionTypeId ?? ClientInsuranceRpaConfiguration.TransactionTypeId;
                    existingClientInsuranceRpaConfiguration.ExternalId = command.ExternalId ?? existingClientInsuranceRpaConfiguration.ExternalId;
                    existingClientInsuranceRpaConfiguration.ClientRpaCredentialConfigurationId = command.ClientRpaCredentialConfigurationId;
                    existingClientInsuranceRpaConfiguration.AlternateClientRpaCredentialConfigurationId = command.AlternateClientRpaCredentialConfigurationId == 0 ? null : command.AlternateClientRpaCredentialConfigurationId;
                    existingClientInsuranceRpaConfiguration.ClientLocationId = command.ClientLocationId; //EN-409
                    await _ClientInsuranceRpaConfigurationRepository.UpdateAsync(existingClientInsuranceRpaConfiguration);
                    await _unitOfWork.Commit(cancellationToken);
                }
                catch (Exception ex)
                {
                    throw new Exception();
                }

                return Result<int>.Success(existingClientInsuranceRpaConfiguration.Id, "ClientInsuranceRpaConfiguration Updated");
            }
        }
    }
}
