using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands
{
    public class CreateClientRpaCredentialConfigCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int RpaInsuranceGroupId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FailureMessage { get; set; }
        public string ReportFailureToEmail { get; set; }
        public bool ExpiryWarningReported { get; set; } = false;
        public bool UseOffHoursOnly { get; set; } = false;
        public string OtpForwardFromEmail { get; set; }
        public int CurrentDailyClaimCount { get; set; }
        public int DailyClaimLimit { get; set; }
        public bool FailureReported { get; set; }
    }

    public class CreateClientRpaCredentialConfigCommandHandler : IRequestHandler<CreateClientRpaCredentialConfigCommand, Result<int>>
    {
        private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateClientRpaCredentialConfigCommandHandler> _localizer;
        private IUnitOfWork<int> _unitOfWork { get; set; }

        public CreateClientRpaCredentialConfigCommandHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<CreateClientRpaCredentialConfigCommandHandler> localizer)
        {
            _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(CreateClientRpaCredentialConfigCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var clientRpaCredentialConfigExists = await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().Entities
                                                                       .Where(crc => crc.Username == request.Username 
                                                                       && crc.RpaInsuranceGroupId == request.RpaInsuranceGroupId).FirstOrDefaultAsync();

                //Prevent adding RpaCredentialCOnfiguration for same username and rpaGroupId
                if (clientRpaCredentialConfigExists != null) 
                {
                    return await Result<int>.FailAsync("Configuration already exists!");
                }

                var clientRpaCredentialConfiguration = _mapper.Map<ClientRpaCredentialConfiguration>(request);
                await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().AddAsync(clientRpaCredentialConfiguration);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(clientRpaCredentialConfiguration.Id, _localizer["RPA Credential Config Saved"]);
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer["Failed to add new!"]);
            }
        }
    }
}
