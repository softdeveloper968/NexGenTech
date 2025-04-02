using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Requests;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientFeeSchedule.Commands.AddEdit
{
	public partial class AddEditClientFeeScheduleCommand : ClientFeeScheduleBase, IRequest<Result<int>>
	{
		public UploadRequest UploadRequest { get; set; }
		public int CopyClientFeeScheduleId { get; set; }
		//public string URL { get; set; }
	}

	public class AddEditClientFeeScheduleCommandHandler : IRequestHandler<AddEditClientFeeScheduleCommand, Result<int>>
	{
		private readonly IMapper _mapper;
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IStringLocalizer<AddEditClientFeeScheduleCommandHandler> _localizer;
		private readonly ICurrentUserService _currentUserService;
		private readonly IInputDataService _inputDataService;
		private readonly IManuallyRunJobService _manuallyRunJobService;
		private readonly ITenantInfo _tenantInfo;
		private int _clientId => _currentUserService.ClientId;

		public AddEditClientFeeScheduleCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditClientFeeScheduleCommandHandler> localizer, ICurrentUserService currentUserService, IInputDataService inputDataService, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IClientFeeScheduleService processFeeScheduleMatchedClaimService, IManuallyRunJobService manuallyRunJobService, ITenantInfo tenantInfo)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_localizer = localizer;
			_currentUserService = currentUserService;
			_inputDataService = inputDataService;
			_manuallyRunJobService = manuallyRunJobService;
			_tenantInfo = tenantInfo;
		}

		public async Task<Result<int>> Handle(AddEditClientFeeScheduleCommand command, CancellationToken cancellationToken)
		{
			command.ClientId = _clientId;
			try
			{
				if (command.Id == 0)
				{
					List<Domain.Entities.ClientFeeSchedule> feeSchedulesByClient = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>()
																						.Entities
																						.Include(x => x.ClientInsuranceFeeSchedules)
																						.Include(x => x.ClientFeeScheduleProviderLevels)
																						.Include(x => x.ClientFeeScheduleSpecialties)
																						.Where(x => x.ClientId == command.ClientId)
																						.ToListAsync();

					if (feeSchedulesByClient != null)
					{
						foreach (var clientFeeSchedule in feeSchedulesByClient)
						{
							if (IsDuplicateEntry(clientFeeSchedule, command))
							{
								return await Result<int>.FailAsync(_localizer["Duplicate entry found"]);
							}
						}
					}

					// Map the command data to a new ClientFeeSchedule entity
					var feeSchedule = _mapper.Map<Domain.Entities.ClientFeeSchedule>(command);


					// Add the new fee schedule entity to the repository and save changes
					await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().AddAsync(feeSchedule);
					await _unitOfWork.Commit(cancellationToken);

					return await Result<int>.SuccessAsync(feeSchedule.Id, _localizer["Client Fee Schedule Saved"]);
				}
				else
				{
					var dbFeeSchedule = await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().Entities
						.Include(x => x.ClientInsuranceFeeSchedules)
						.Include(x => x.ClientFeeScheduleProviderLevels)
						.Include(x => x.ClientFeeScheduleSpecialties)
						.FirstOrDefaultAsync(x => x.Id == command.Id);

					if (dbFeeSchedule != null)
					{
						_mapper.Map(command, dbFeeSchedule);
						await _unitOfWork.Repository<Domain.Entities.ClientFeeSchedule>().UpdateAsync(dbFeeSchedule);
						await _unitOfWork.Commit(cancellationToken);

						BackgroundJob.Enqueue(() => _manuallyRunJobService.ProcessFeeScheduleEntriesForSingleTenant(_tenantInfo.Identifier, dbFeeSchedule.Id)); //EN-232

						return await Result<int>.SuccessAsync(dbFeeSchedule.Id, _localizer["Client Fee Schedule Updated"]);

					}
					else
					{
						return await Result<int>.FailAsync(_localizer["Client Fee Schedule Not Found"]);
					}
				}
			}
			catch (Exception e)
			{
				return await Result<int>.FailAsync(_localizer[e.Message]);
			}
		}

		// Method to check if a duplicate entry exists for the given command and fee schedules
		private bool IsDuplicateEntry(Domain.Entities.ClientFeeSchedule clientFeeSchedule, AddEditClientFeeScheduleCommand command)
		{
			// Check if the start and end dates match
			if (clientFeeSchedule.StartDate != command.StartDate || clientFeeSchedule.EndDate != command.EndDate)
			{
				return false;
			}

			// Check if specialty IDs match
			var specialtyIds = clientFeeSchedule.ClientFeeScheduleSpecialties.Select(x => (int)x.SpecialtyId);
			var commandSpecialtyIds = command.ClientFeeScheduleSpecialties.Select(x => (int)x.SpecialtyId);
			if (!specialtyIds.SequenceEqual(commandSpecialtyIds))
			{
				return false;
			}

			// Check if provider level IDs match
			var providerLevelIds = clientFeeSchedule.ClientFeeScheduleProviderLevels.Select(x => (int)x.ProviderLevelId);
			var commandProviderLevelIds = command.ClientFeeScheduleProviderLevels.Select(x => (int)x.ProviderLevelId);
			if (!providerLevelIds.SequenceEqual(commandProviderLevelIds))
			{
				return false;
			}

			// Check if client insurance IDs match
			var clientInsuranceIds = clientFeeSchedule.ClientInsuranceFeeSchedules.Select(x => x.ClientInsuranceId);
			var commandInsuranceIds = command.ClientInsuranceFeeSchedules.Select(x => x.ClientInsuranceId);
			if (!clientInsuranceIds.SequenceEqual(commandInsuranceIds))
			{
				return false;
			}

			// If all checks pass, it's a duplicate
			return true;
		}

	}
}
