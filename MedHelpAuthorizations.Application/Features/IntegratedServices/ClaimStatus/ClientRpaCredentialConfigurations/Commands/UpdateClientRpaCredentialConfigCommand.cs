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
    public class UpdateClientRpaCredentialConfigCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int RpaInsuranceGroupId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        //public string FailureMessage { get; set; }
        public string ReportFailureToEmail { get; set; }
        //public bool ExpiryWarningReported { get; set; } = false;
        public bool? UseOffHoursOnly { get; set; }
        public string OtpForwardFromEmail { get; set; }
        public int? CurrentDailyClaimCount { get; set; }
        public int? DailyClaimLimit { get; set; }
    }
    public class UpdateClientRpaCredentialConfigCommandHandler : IRequestHandler<UpdateClientRpaCredentialConfigCommand, Result<int>>
    {
        private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<UpdateClientRpaCredentialConfigCommandHandler> _localizer;
        private IUnitOfWork<int> _unitOfWork { get; set; }

        public UpdateClientRpaCredentialConfigCommandHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<UpdateClientRpaCredentialConfigCommandHandler> localizer)
        {
            _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(UpdateClientRpaCredentialConfigCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var clientRpaCredentialConfiguration = await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().GetByIdAsync(request.Id);
                
                if(request.RpaInsuranceGroupId != clientRpaCredentialConfiguration.RpaInsuranceGroupId)
                {
                    return await Result<int>.FailAsync(_localizer["RpaGroupId Updates are not allowed."]);
                }

                if (clientRpaCredentialConfiguration != null)
                {
                    var clientRpaCredentialConfigExists = await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().Entities
                                                                       .Where(crc => crc.Username == request.Username
                                                                       && crc.RpaInsuranceGroupId == request.RpaInsuranceGroupId
                                                                       && crc.Id != clientRpaCredentialConfiguration.Id).FirstOrDefaultAsync();

                    //Prevent adding RpaCredentialCOnfiguration for same username and rpaGroupId
                    if (clientRpaCredentialConfigExists == null)
                    {
                        if ((!string.IsNullOrWhiteSpace(request.Username) && request.Username != clientRpaCredentialConfiguration.Username) || (!string.IsNullOrWhiteSpace(request.Password) && request.Password != clientRpaCredentialConfiguration.Password))
                        {
                            clientRpaCredentialConfiguration.FailureReported = false;
                            clientRpaCredentialConfiguration.FailureMessage = string.Empty;
                            clientRpaCredentialConfiguration.ExpiryWarningReported = false;
                        }
                        //_mapper.Map(request, clientRpaCredentialConfiguration);
                        clientRpaCredentialConfiguration.Username = request.Username ?? clientRpaCredentialConfiguration.Username;
                        clientRpaCredentialConfiguration.Password = request.Password ?? clientRpaCredentialConfiguration.Password;
                        clientRpaCredentialConfiguration.UseOffHoursOnly = request.UseOffHoursOnly ?? clientRpaCredentialConfiguration.UseOffHoursOnly;
                        clientRpaCredentialConfiguration.OtpForwardFromEmail = request.OtpForwardFromEmail ?? clientRpaCredentialConfiguration.OtpForwardFromEmail;
                        clientRpaCredentialConfiguration.ReportFailureToEmail = request.ReportFailureToEmail ?? clientRpaCredentialConfiguration.ReportFailureToEmail;
                        //clientRpaCredentialConfiguration.DailyClaimCount = request.DailyClaimLimit; 
                        //clientRpaCredentialConfiguration.CurrentDayCLaimCount = request.CurrentDailyClaimCount;

                        await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().UpdateAsync(clientRpaCredentialConfiguration);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(clientRpaCredentialConfiguration.Id, _localizer["RPA Credential Config Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["RPA Credential Config already exists!"]);
                    }
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["RPA Credential Config Not Found!"]);
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer["Failed to add new!"]);
            }
        }
    }
}
