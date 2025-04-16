using AutoMapper;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.UpsertAddresses;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Cardholders.Commands.AddEdit
{
    public partial class AddEditCardholderCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string Email { get; set; }
        public int AddressId { get; set; }
        public AddressTypeEnum AddressTypeId { get; set; }
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
        public string ExternalId { get; set; }

    }

    public class AddEditCardholderCommandHandler : IRequestHandler<AddEditCardholderCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<AddEditCardholderCommandHandler> _localizer;
        private int _clientId => _currentUserService.ClientId;
        public AddEditCardholderCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IMediator mediator, ICurrentUserService currentUserService, IStringLocalizer<AddEditCardholderCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditCardholderCommand command, CancellationToken cancellationToken)
        {
            try
            {
                //upsert Address
                var upsertAddressCommand = _mapper.Map<UpsertAddressCommand>(command);
                var addressIdResult = await _mediator.Send(upsertAddressCommand);
                command.AddressId = addressIdResult.Data;

                // Upsert Person
                var upsertPersonCommand = _mapper.Map<UpsertPersonCommand>(command);
                var personIdResult = await _mediator.Send(upsertPersonCommand);
                command.PersonId = personIdResult.Data;

                if (command.Id == 0)
                {
                    var cardholder = _mapper.Map<Cardholder>(command);
                    cardholder.ClientId = _clientId;
                    await _unitOfWork.Repository<Cardholder>().AddAsync(cardholder);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(cardholder.Id, _localizer["Cardholder Saved"]);
                }
                else
                {
                    var cardholder = await _unitOfWork.Repository<Cardholder>().GetByIdAsync(command.Id);
                    if (cardholder != null)
                    {
                        cardholder.Person.DateOfBirth = command.DateOfBirth;
                        cardholder.Person.FirstName = command.FirstName;
                        cardholder.Person.LastName = command.LastName;

                        await _unitOfWork.Repository<Cardholder>().UpdateAsync(cardholder);
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(cardholder.Id, _localizer["Cardholder Updated"]);
                    }
                    else
                    {
                        return await Result<int>.FailAsync(_localizer["Cardholder Not Found!"]);
                    }
                }
            }
            catch(Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
