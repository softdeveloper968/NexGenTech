using AutoMapper;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.UpsertAddresses;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Commands.AddEdit
{
    public partial class AddEditClientLocationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long? OfficePhoneNumber { get; set; }
        public long? OfficeFaxNumber { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public int ClientId { get; set; }
        public List<SpecialtyEnum> ClientLocationSpecialtyId { get; set; } = new List<SpecialtyEnum>();
        public string Npi { get; set; }
        public int? EligibilityLocationId { get; set; }

    }

    public class AddEditClientLocationCommandHandler : IRequestHandler<AddEditClientLocationCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientLocationCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IClientLocationServiceTypeRepository _clientLocationServiceTypeRepository;
        private readonly IClientLocationSpecialityRepository _clientLocationSpecialityRepository;
        private int _clientId => _currentUserService.ClientId;
        private readonly IMediator _mediator;

        public AddEditClientLocationCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IMediator mediator, ICurrentUserService currentUserService, IStringLocalizer<AddEditClientLocationCommandHandler> localizer, IClientLocationServiceTypeRepository clientLocationServiceTypeRepository
            , IClientLocationSpecialityRepository clientLocationSpecialityRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _mediator = mediator;
            _clientLocationServiceTypeRepository = clientLocationServiceTypeRepository;
            _clientLocationSpecialityRepository= clientLocationSpecialityRepository;
        }

        public async Task<Result<int>> Handle(AddEditClientLocationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientId = _clientId;
                if(command.Address != null)
                    command.Address.ClientId = _clientId;

                var upsertAddressCommand = _mapper.Map<UpsertAddressCommand>(command.Address);

                //check for duplicate name
                if (command.Id == 0)
                {
                    var clientLocation = await _unitOfWork.Repository<ClientLocation>()
                       .Entities
                       .FirstOrDefaultAsync(l => l.Name == command.Name && l.ClientId == command.ClientId, cancellationToken: cancellationToken);
                    if (clientLocation != null)
                    {
                        return await Result<int>.FailAsync(_localizer["ClientLocation With Same Name Already Exists!"]);
                    }
                    else
                    {
                        //upsert Address
                        var addressIdResult = await _mediator.Send(upsertAddressCommand);
                        command.AddressId = addressIdResult.Data;

                        clientLocation = _mapper.Map<ClientLocation>(command);
                        await _unitOfWork.Repository<ClientLocation>().AddAsync(clientLocation);
                        await _unitOfWork.Commit(cancellationToken);

                        ///Add ClientlocationSpeciality, if any selected.
                        if (command.ClientLocationSpecialtyId is not null && command.ClientLocationSpecialtyId.Any())
                        {
                            foreach (var item in command.ClientLocationSpecialtyId)
                            {
                                var locationSpeciality = new Domain.Entities.ClientLocationSpeciality
                                {
									SpecialityId = item,
                                    ClientLocationId = clientLocation.Id,
                                    ClientId = _clientId
                                };
                                await _unitOfWork.Repository<Domain.Entities.ClientLocationSpeciality>().AddAsync(locationSpeciality);
                            }
                        }

                        await _unitOfWork.Commit(cancellationToken);
                        //return await Result<int>.SuccessAsync(_localizer["ClientLocation Saved"]);
                        return await Result<int>.SuccessAsync(clientLocation.Id, _localizer["ClientLocation Saved"]);
                    }
                }
                else
                {
                    var clientLocation = await _unitOfWork.Repository<ClientLocation>().GetByIdAsync(command.Id);
                    if (clientLocation == null)
                    {
                        return await Result<int>.FailAsync(_localizer["ClientLocation Not Found!"]);
                    }
                    else
                    {
                        //upsert Address
                        var addressIdResult = await _mediator.Send(upsertAddressCommand);
                        command.AddressId = addressIdResult.Data;

                        //_mapper.Map(command, clientLocation);
                        clientLocation.Name = command.Name;
                        clientLocation.OfficePhoneNumber = command.OfficePhoneNumber;
                        clientLocation.OfficeFaxNumber = command.OfficeFaxNumber;
                        clientLocation.Npi = command.Npi;
                        clientLocation.EligibilityLocationId = command.EligibilityLocationId;

                        await _unitOfWork.Repository<ClientLocation>().UpdateAsync(clientLocation);

						/// When updating first delete all Entries from ClientLocationSpeciality table for locationId, clientId.
						/// Then Add selected ClientLocationSpeciality entry.
						/// Check if any location ClientLocationSpeciality entry then first delete All then make a new entries.
                        /// 
						var locationSpecialties = await _clientLocationSpecialityRepository.GetClientLocationsSpecialityByLocationId(_clientId, clientLocation.Id);

                         _unitOfWork.Repository<Domain.Entities.ClientLocationSpeciality>().RemoveRange(locationSpecialties);
                        if (command.ClientLocationSpecialtyId.Any())
                        {
							// Create a list to hold the new location specialties
							var newLocationSpecialties = new List<Domain.Entities.ClientLocationSpeciality>();

							foreach (var item in command.ClientLocationSpecialtyId)
							{
								var locationSpeciality = new Domain.Entities.ClientLocationSpeciality
								{
									SpecialityId = item,
									ClientLocationId = clientLocation.Id,
									ClientId = _clientId
								};
								newLocationSpecialties.Add(locationSpeciality);
							}

							// Use AddRange to add the new location specialties
							_unitOfWork.Repository<Domain.Entities.ClientLocationSpeciality>().AddRange(newLocationSpecialties);

						}

						await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(clientLocation.Id, _localizer["ClientLocation Updated"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[$"{ex.Message}--\r\n--{ex.StackTrace}"]);
            }
        }
    }
}
