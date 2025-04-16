using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Client_ApplicationFeatures;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using static MedHelpAuthorizations.Shared.Constants.Application.ApplicationConstants;

namespace MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit
{
    public class AddEditClientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ClientCode { get; set; }
        public long? PhoneNumber { get; set; } = default(long?);
        public long? FaxNumber { get; set; } = default(long?);
        public int? ClientKpiId { get; set; } = default(int?);
        public ClientKpiDto ClientKpi { get; set; }
        [Required]
        public List<ClientApplicationFeatureDto> ClientApplicationFeatures { get; set; } = new();
        public List<int> EmployeeClientIds { get; set; } = new ();
        public List<ClientAuthTypeDto> ClientAuthTypes { get; set; }=new ();
        public List<ClientInsuranceDto> ClientInsurances { get; set; } = new();
        public List<ClientLocationDto> ClientLocations { get; set; } = new();
        public List<int> CurrentlyAssignedEmployees { get; set; } = new();
        public List<ClientApiIntegrationKeyDto> ClientApiIntegrationKeys { get; set; } = new();
		public SourceSystemEnum? SourceSystemId { get; set; }
        public List<HolidaysEnum>? DaysOfOperations { get; set; } = new();
		public List<ClientSpecialityDto> ClientSpecialties { get; set; } = new List<ClientSpecialityDto>();
		//public List<AddClientKpiData> AddClientKpisData { get; set; } = new List<AddClientKpiData>();

		//public class AddClientKpiData
		//{
		//    public EmployeeConfigData EmployeeClientConfigs { get; set; }
		//    public AddEditClientLevelKpiViewModel clientLevelKpis { get; set; }
		//}

        public List<ClientHoliday> ClientHolidays { get; set; } = new();
        public List<ClientDayOfOperation> ClientDaysOfOperation { get; set; } = new();
        public int? TaxId { get; set; } //EN-538
        public int? AutoLogMinutes { get; set; }
    }

    public class AddEditClientCommandHandler : IRequestHandler<AddEditClientCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClientRepository _clientRepository;
        private readonly IClientAuthTypesRepository _clientAuthTypeRepository;
        private readonly IClientApplicationFeatureRepository _clientApplicationFeatureRepository;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditClientCommandHandler> _localizer;
		private readonly ICurrentUserService _currentUserService;

		private int _clientId => _currentUserService.ClientId;

		public AddEditClientCommandHandler(IUnitOfWork<int> unitOfWork, IClientRepository clientRepository, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditClientCommandHandler> localizer, IClientAuthTypesRepository clientAuthTypeRepository, IClientApplicationFeatureRepository clientApplicationFeatureRepository,
			ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _clientRepository = clientRepository;
            _clientAuthTypeRepository = clientAuthTypeRepository;
            _clientApplicationFeatureRepository = clientApplicationFeatureRepository;
			_currentUserService = currentUserService;

        }

        public async Task<Result<int>> Handle(AddEditClientCommand command, CancellationToken cancellationToken)
        {
            Domain.Entities.Client dbClient = null;

            try
            {
                if (command.Id == 0)
                {
                    Domain.Entities.Client clientdata = new Domain.Entities.Client()
                    {
                        ClientCode = command.ClientCode,
                        PhoneNumber = command.PhoneNumber,
                        FaxNumber = command.FaxNumber,
                        Name = command.Name,
                        SourceSystemId = command.SourceSystemId,
                        TaxId = command.TaxId,
                        AutoLogMinutes = command.AutoLogMinutes,
                    };
                    dbClient = await _unitOfWork.Repository<Domain.Entities.Client>().AddAsync(clientdata);
                    dbClient.ClientAuthTypes = _mapper.Map<List<ClientAuthType>>(command.ClientAuthTypes);
                    dbClient.ClientApplicationFeatures = _mapper.Map<List<ClientApplicationFeature>>(command.ClientApplicationFeatures);

                    //add the default docuement type while adding the new client
                    dbClient.DocumentTypes = DocumentTypeConstants.GetDefaults().Select(x =>
                        new DocumentType() { Name = x.Item1, Description = x.Item2 }).ToList();

                    await _unitOfWork.Commit(cancellationToken);

					if (command.ClientSpecialties!= null && command.ClientSpecialties.Any())
					{
						List<ClientSpecialty> clientSpecialties = new List<ClientSpecialty>();

						foreach (var item in command.ClientSpecialties)
						{
							ClientSpecialty clientSpecialty = new ClientSpecialty()
							{
								SpecialtyId = item.SpecialtyId,
								ClientId = clientdata.Id
							};

							clientSpecialties.Add(clientSpecialty);
						}
						_unitOfWork.Repository<ClientSpecialty>().AddRange(clientSpecialties);
						await _unitOfWork.Commit(cancellationToken);
					}

                    //if (command.ClientHolidays != null && command.ClientHolidays.Any())
                    //{
                    //    _unitOfWork.Repository<ClientHoliday>().AddRange(command.ClientHolidays);
                    //    await _unitOfWork.Commit(cancellationToken);
                    //}

                    //if (command.ClientDaysOfOperation != null && command.ClientDaysOfOperation.Any())
                    //{
                    //    _unitOfWork.Repository<ClientDayOfOperation>().AddRange(command.ClientDaysOfOperation);
                    //    await _unitOfWork.Commit(cancellationToken);
                    //}

                    return await Result<int>.SuccessAsync(clientdata.Id, _localizer["Client Saved"]);

                }
                else
                {
                    var clientData = await _clientRepository.GetClientDatById(command.Id);
                    if (clientData == null)
                        return await Result<int>.FailAsync(_localizer["Client Not Found!"]);

                    // Update basic properties
                    clientData.Name = command.Name;
                    clientData.ClientCode = command.ClientCode;
                    clientData.PhoneNumber = command.PhoneNumber;
                    clientData.FaxNumber = command.FaxNumber;
                    clientData.SourceSystemId = command.SourceSystemId;
                    clientData.TaxId = command.TaxId;
                    clientData.AutoLogMinutes = command.AutoLogMinutes;
                    clientData.ClientKpiId = command.ClientKpiId != 0 ? command.ClientKpiId : null;

                    // Update holidays
                    await UpdateClientHolidays(clientData.Id, command.ClientHolidays, cancellationToken);

                    // Update days of operation  
                    await UpdateClientDaysOfOperation(clientData.Id, command.ClientDaysOfOperation, cancellationToken);

                    // Update specialties
                    await UpdateClientSpecialties(clientData.Id, command.ClientSpecialties, cancellationToken);

                    // Update auth types and app features
                    await UpdateClientAuthTypes(clientData, command.ClientAuthTypes);
                    await UpdateClientAppFeatures(clientData, command.ClientApplicationFeatures);

                    await _unitOfWork.Repository<Domain.Entities.Client>().UpdateAsync(clientData);
                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(clientData.Id, _localizer["Client Updated"]);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task UpdateClientHolidays(int clientId, List<ClientHoliday> newHolidays, CancellationToken cancellationToken)
        {
            var existingHolidays = await _unitOfWork.Repository<ClientHoliday>().Entities.Where(x => x.ClientId == clientId).ToListAsync();
            if (existingHolidays.Any())
            {
                _unitOfWork.Repository<ClientHoliday>().RemoveRange(existingHolidays);
                await _unitOfWork.Commit(cancellationToken);
            }

            if (newHolidays?.Any() == true)
            {
                _unitOfWork.Repository<ClientHoliday>().AddRange(newHolidays);
                await _unitOfWork.Commit(cancellationToken);
            }
        }

        private async Task UpdateClientDaysOfOperation(int clientId, List<ClientDayOfOperation> newDaysOfOperation, CancellationToken cancellationToken)
        {
            var existingDays = await _unitOfWork.Repository<ClientDayOfOperation>().Entities.Where(x => x.ClientId == clientId).ToListAsync();
            if (existingDays?.Any() == true)
            {
                _unitOfWork.Repository<ClientDayOfOperation>().RemoveRange(existingDays);
                await _unitOfWork.Commit(cancellationToken);
            }

            if (newDaysOfOperation?.Any() == true)
            {
                _unitOfWork.Repository<ClientDayOfOperation>().AddRange(newDaysOfOperation);
                await _unitOfWork.Commit(cancellationToken);
            }
        }

        private async Task UpdateClientSpecialties(int clientId, List<ClientSpecialityDto> newSpecialties, CancellationToken cancellationToken)
        {
            if (!newSpecialties.Any()) return;

            var existingSpecialties = await _unitOfWork.Repository<ClientSpecialty>().Entities.Where(x => x.ClientId == clientId).ToListAsync();

            if (existingSpecialties.Any())
            {
                _unitOfWork.Repository<ClientSpecialty>().RemoveRange(existingSpecialties);
                await _unitOfWork.Commit(cancellationToken);
            }

            var clientSpecialties = newSpecialties.Select(item => new ClientSpecialty
            {
                SpecialtyId = item.SpecialtyId,
                ClientId = clientId
            }).ToList();

            _unitOfWork.Repository<ClientSpecialty>().AddRange(clientSpecialties);
            await _unitOfWork.Commit(cancellationToken);
        }

        private async Task UpdateClientAuthTypes(Domain.Entities.Client dbClient, List<ClientAuthTypeDto> newAuthTypes)
        {
            var authTypesToRemove = dbClient.ClientAuthTypes.Where(item => !newAuthTypes.Any(newItem => newItem.AuthTypeId == item.AuthTypeId));
            var authTypesToAdd = newAuthTypes.Where(item => !dbClient.ClientAuthTypes.Any(existingItem => existingItem.AuthTypeId == item.AuthTypeId));

            _unitOfWork.Repository<ClientAuthType>().RemoveRange(authTypesToRemove);
            _unitOfWork.Repository<ClientAuthType>().AddRange(_mapper.Map<List<ClientAuthType>>(authTypesToAdd));
        }

        private async Task UpdateClientAppFeatures(Domain.Entities.Client dbClient, List<ClientApplicationFeatureDto> newFeatures)
        {
            var featuresToRemove = dbClient.ClientApplicationFeatures.Where(item => !newFeatures.Any(newItem => newItem.ApplicationFeatureId == item.ApplicationFeatureId));
            var featuresToAdd = newFeatures.Where(item => !dbClient.ClientApplicationFeatures.Any(existingItem => existingItem.ApplicationFeatureId == item.ApplicationFeatureId));

            _unitOfWork.Repository<ClientApplicationFeature>().RemoveRange(featuresToRemove);
            _unitOfWork.Repository<ClientApplicationFeature>().AddRange(_mapper.Map<List<ClientApplicationFeature>>(featuresToAdd));
        }
    }
}