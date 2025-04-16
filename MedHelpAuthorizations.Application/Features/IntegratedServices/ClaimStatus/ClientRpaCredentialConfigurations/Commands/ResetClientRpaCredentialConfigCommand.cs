using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Commands
{
	public class ResetClientRpaCredentialConfigCommand : IRequest<Result<int>>
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
	}

	public class ResetClientRpaCredentialConfigCommandHandler : IRequestHandler<ResetClientRpaCredentialConfigCommand, Result<int>>
	{
		private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
		private readonly IMapper _mapper;
		private readonly IStringLocalizer<UpdateClientRpaCredentialConfigCommandHandler> _localizer;
		private IUnitOfWork<int> _unitOfWork { get; set; }

		public ResetClientRpaCredentialConfigCommandHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<UpdateClientRpaCredentialConfigCommandHandler> localizer)
		{
			_clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizer = localizer;
		}

		public async Task<Result<int>> Handle(ResetClientRpaCredentialConfigCommand request, CancellationToken cancellationToken)
		{
			try
			{
				var clientRpaCredentialConfiguration = await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().GetByIdAsync(request.Id);

				if (clientRpaCredentialConfiguration != null)
				{
					clientRpaCredentialConfiguration.FailureReported = false;
					clientRpaCredentialConfiguration.FailureMessage = string.Empty;
					clientRpaCredentialConfiguration.ExpiryWarningReported = false;
					_mapper.Map(request, clientRpaCredentialConfiguration);

					await _unitOfWork.Repository<ClientRpaCredentialConfiguration>().UpdateAsync(clientRpaCredentialConfiguration);
					await _unitOfWork.Commit(cancellationToken);
					return await Result<int>.SuccessAsync(clientRpaCredentialConfiguration.Id, _localizer["Configuration Reset Successfully"]);
				}
				else
				{
					return await Result<int>.FailAsync(_localizer["Configuratio Not Found!"]);
				}
			}
			catch (Exception ex)
			{
				return await Result<int>.FailAsync(_localizer["Configuration Reset Failed"]);
			}
		}
	}
}
