using AutoMapper;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Providers.Commands.AddEdit
{
    public class AddEditProviderCommand : AddEditProviderCommandBase, IRequest<Result<int>>
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int ClientId { get; set; }
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string Email { get; set; }
        public int? AddressId { get; set; }
        public AddressTypeEnum AddressTypeId { get; set; } = AddressTypeEnum.Residential;
        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }
        public string City { get; set; }
        public StateEnum StateId { get; set; } = StateEnum.UNK;
        public string PostalCode { get; set; }
        public bool Normalized { get; set; }
        public int DeliveryPointBarcode { get; set; }
        public GenderIdentityEnum? GenderIdentityId { get; set; }
        //public AdministrativeGenderEnum? AdministrativeGenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? SocialSecurityNumber { get; set; }
        //AA-106
        public IEnumerable<int> ProviderLocationIds { get; set; }
        //EN-238
        public int ScheduledVisitsPerDayKpi { get; set; }
        public int PatientsSeenPerDayKpi { get; set; }
        public int DaysToBillKpi { get; set; }
        public decimal? NoShowRateKpi { get; set; }
    }

    public class AddEditProviderCommandHandler : IRequestHandler<AddEditProviderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditProviderCommandHandler> _localizer;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public AddEditProviderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IMediator mediator, ICurrentUserService currentUserService, IStringLocalizer<AddEditProviderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditProviderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientId = _clientId;

                var data = await _unitOfWork.Repository<ClientProvider>()
                    .Entities
                    .Include(x => x.Person)
                    .FirstOrDefaultAsync(p => p.ClientId == command.ClientId
                                            && ((p.Person.FirstName == command.FirstName
                                            && p.Person.LastName == command.LastName)
                                            || p.Npi == command.Npi));

                if (command.ProviderId == 0 && data != null)
                {
                    string message = _localizer["Provider Already Exists!"];
                    if (command.ProviderId == 0 && command.Npi == data.Npi)
                    {
                        message = _localizer["Provider NPI Already Exists!"];
                    }
                    return await Result<int>.FailAsync(message);
                }
                else
                {
                    //upsert Address
                    //var upsertAddressCommand = _mapper.Map<UpsertAddressCommand>(command);
                    //var addressIdResult = await _mediator.Send(upsertAddressCommand);
                    //command.AddressId = addressIdResult.Data;

                    // Upsert Person
                    var upsertPersonCommand = _mapper.Map<UpsertPersonCommand>(command);
                    var personIdResult = await _mediator.Send(upsertPersonCommand);
                    command.PersonId = personIdResult.Data;

                    if (command.ProviderId == 0)
                    {
                        //var provider = _mapper.Map<ClientProvider>(command);
                        //List<ClientProviderLocation> providerLocations = new();
                        // EN-2
                        var provider = new ClientProvider
                        {
                            ClientId = command.ClientId,
                            PersonId = command.PersonId,
                            SpecialtyId = command.SpecialtyId.Value,
                            ProviderLevelId = command.ProviderLevelId,
                            Credentials = command.Credentials,
                            Npi = command.Npi,
                            License = command.License,
                            ScheduledVisitsPerDayKpi = command.ScheduledVisitsPerDayKpi,
                            PatientsSeenPerDayKpi = command.PatientsSeenPerDayKpi,
                            DaysToBillKpi = command.DaysToBillKpi,
                            NoShowRateKpi = command.NoShowRateKpi ?? 0.0m,
                        };
                        await _unitOfWork.Repository<ClientProvider>().AddAsync(provider);
                        await _unitOfWork.Commit(cancellationToken);
                        if (command.ProviderLocationIds != null)
                        {
                            command.ProviderLocationIds.ToList().ForEach(async id =>
                            {
                                var providerLocation = new ClientProviderLocation()
                                {
                                    ClientProviderId = provider.Id,
                                    ClientLocationId = id
                                };
                                await _unitOfWork.Repository<ClientProviderLocation>().AddAsync(providerLocation);
                                //providerLocations.Add(providerLocation);
                            });
                            await _unitOfWork.Commit(cancellationToken);
                        }

                        return await Result<int>.SuccessAsync(provider.Id, _localizer["Provider Saved"]);
                    }
                    else
                    {
                        var provider = await _unitOfWork.Repository<ClientProvider>().Entities
                            .Include(p => p.ClientProviderLocations)
                            .FirstOrDefaultAsync(c => c.Id == command.ProviderId);
                        if (provider != null)
                        {
                            provider.Npi = command.Npi ?? provider.Npi;
                            provider.License = command.License ?? provider.License;
                            provider.Person.FirstName = command.FirstName ?? command.FirstName;
                            provider.SpecialtyId = command.SpecialtyId.Value; //EN-71
                            provider.ProviderLevelId = command.ProviderLevelId;
                            provider.ScheduledVisitsPerDayKpi = command.ScheduledVisitsPerDayKpi;
                            provider.PatientsSeenPerDayKpi = command.PatientsSeenPerDayKpi;
                            provider.DaysToBillKpi = command.DaysToBillKpi;
                            provider.NoShowRateKpi = command.NoShowRateKpi ?? 0;

                            await _unitOfWork.Repository<ClientProvider>().UpdateAsync(provider);

                            //existing locations assigned to the provider
                            var existingLocations = provider.ClientProviderLocations.Select(l => l.ClientLocationId).ToList();

                            //new locations to the provider that were not in the records
                            var newLocations = command.ProviderLocationIds.Where(l => !existingLocations.Contains(l)).ToList();

                            //var removedLocationsId = existingLocations.Where(l => !command.ProviderLocationIds.Contains(l)).ToList();

                            //removed locations that are in the records but not in the update command
                            var removedLocations = provider.ClientProviderLocations.Where(l => !command.ProviderLocationIds.Contains(l.ClientLocationId)).ToList();

                            //remove locations that were in the records but are not in the update command
                            removedLocations.ForEach(async l =>
                            {
                                await _unitOfWork.Repository<ClientProviderLocation>().DeleteAsync(l);

                            });

                            //insert all the new added locations
                            newLocations.ToList().ForEach(async id =>
                            {
                                var providerLocation = new ClientProviderLocation()
                                {
                                    ClientProviderId = provider.Id,
                                    ClientLocationId = id
                                };
                                await _unitOfWork.Repository<ClientProviderLocation>().AddAsync(providerLocation);
                                //providerLocations.Add(providerLocation);
                            });
                            await _unitOfWork.Commit(cancellationToken);
                            return await Result<int>.SuccessAsync(provider.Id, _localizer["Provider Updated"]);
                        }
                        else
                        {
                            return await Result<int>.FailAsync(_localizer["Provider Not Found!"]);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                return await Result<int>.FailAsync(ex.Message);
            }
        }

    }
}
