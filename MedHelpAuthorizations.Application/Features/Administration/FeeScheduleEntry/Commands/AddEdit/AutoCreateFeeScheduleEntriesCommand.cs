using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit
{
	public class AutoCreateFeeScheduleEntriesCommand : FeeScheduleCriteriaModel, IRequest<Result<int>>
	{
	}

	public class AutoCreateFeeScheduleEntriesCommandHandler : IRequestHandler<AutoCreateFeeScheduleEntriesCommand, Result<int>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IStringLocalizer<AutoCreateFeeScheduleEntriesCommandHandler> _localizer;
		private readonly ICurrentUserService _currentUserService;
		private readonly IClientFeeScheduleService _clientFeeScheduleService;
		private readonly ITenantInfo _tenantInfo;
		private readonly IManuallyRunJobService _manuallyRunJobService;
		private int _clientId => _currentUserService.ClientId;

		public AutoCreateFeeScheduleEntriesCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AutoCreateFeeScheduleEntriesCommandHandler> localizer, ICurrentUserService currentUserService,
			IClientFeeScheduleService clientFeeScheduleService, ITenantInfo tenantInfo, IManuallyRunJobService manuallyRunJobService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizer = localizer;
			_currentUserService = currentUserService;
			_clientFeeScheduleService = clientFeeScheduleService;
			_tenantInfo = tenantInfo;
			_manuallyRunJobService = manuallyRunJobService;
		}

		public async Task<Result<int>> Handle(AutoCreateFeeScheduleEntriesCommand command, CancellationToken cancellationToken)
		{
			try
			{
				// Set the client ID from the current user
				int clientId = _currentUserService.ClientId;

				if (command.ClientFeeScheduleId != 0)
				{

					// Get fee schedule entries
					var feeScheduleEntries = await _clientFeeScheduleService.GetClaimStatusAveragePaidAmountAsync(command, clientId, _tenantInfo.ConnectionString);

					if (feeScheduleEntries != null)
					{
						// Map fee schedule entries to domain model
						List<ClientFeeScheduleEntry> feeScheduleEntryData = feeScheduleEntries.Select(feeScheduleEntry => new ClientFeeScheduleEntry
						{
							ClientCptCodeId = feeScheduleEntry.ClientCptCodeId,
							Fee = feeScheduleEntry.AverageBilledAmount,
							AllowedAmount = feeScheduleEntry.AverageLineItemPaidAmount,
							IsReimbursable = true,
							ClientFeeScheduleId = command.ClientFeeScheduleId,
						}).ToList();

						// Add fee schedule entries to repository
						if (feeScheduleEntryData.Any())
						{
							_unitOfWork.Repository<ClientFeeScheduleEntry>().AddRange(feeScheduleEntryData);
							await _unitOfWork.Commit(cancellationToken);
						}
						// Schedule background job to process fee schedule entries for a single tenant
						BackgroundJob.Enqueue(() => _manuallyRunJobService.ProcessFeeScheduleEntriesForSingleTenant(_tenantInfo.Identifier, command.ClientFeeScheduleId)); //EN-232
					}

					return await Result<int>.SuccessAsync(command.ClientFeeScheduleId, _localizer["Client Fee Schedule Entries Saved"]);
				}

				return await Result<int>.FailAsync("Error");
			}
			catch (Exception e)
			{
				// Handle exceptions
				return await Result<int>.FailAsync(_localizer[e.Message]);
			}
		}
	}
}
