using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Application.Features.Addresses.Commands.UpsertAddresses;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Application.Extensions;

namespace MedHelpAuthorizations.Application.Features.Patients.Commands.AddEdit
{
    public partial class AddEditPatientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }

        public string ExternalId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public long? HomePhoneNumber { get; set; }

        public long? MobilePhoneNumber { get; set; }

        public long? OfficePhoneNumber { get; set; }

        public string Email { get; set; }

        public int? AddressId { get; set; }

        public string AddressStreetLine1 { get; set; }

        public string AddressStreetLine2 { get; set; }

        public string City { get; set; }

        public StateEnum StateId { get; set; }

        public string PostalCode { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        public GenderIdentityEnum GenderIdentityId { get; set; }

        public AdministrativeGenderEnum AdministrativeGenderId { get; set; }

        public string SocialSecurityNumber { get; set; }
        public string DecryptedSocialSecurityNumber { get; set; }

        public RelationShipTypeEnum ResponsiblePartyRelationshipToPatient { get; set; }

        public int? ResponsiblePartyId { get; set; }

        public int? PrimaryProviderId { get; set; }

        public int? ReferringProviderId { get; set; }

        public DateTime? BenefitsCheckedOn { get; set; }
    }

    public class AddEditPatientCommandHandler : IRequestHandler<AddEditPatientCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditPatientCommandHandler> _localizer;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditPatientCommandHandler(IPatientRepository patientRepository, IUnitOfWork<int> unitOfWork,
                                            IMapper mapper, IUploadService uploadService,
                                            IStringLocalizer<AddEditPatientCommandHandler> localizer,
                                            ICurrentUserService currentUserService, IMediator mediator)
        {
            _patientRepository = patientRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(AddEditPatientCommand command, CancellationToken cancellationToken)
        {
            try
            {
                Patient patient;

                // Upsert Address
                UpsertAddressCommand mappedAddress = _mapper.Map<UpsertAddressCommand>(command);
                var addressIdResult = await _mediator.Send(mappedAddress);
                if (!addressIdResult.Succeeded)
                {
                    return Result<int>.Fail(addressIdResult.Messages);
                }
                int addressId = addressIdResult.Data;

                // Upsert Person
                //command.SocialSecurityNumber = SocialSecurityNumberExtensions.EncryptSSN(command.DecryptedSocialSecurityNumber);
                UpsertPersonCommand mappedPerson = _mapper.Map<UpsertPersonCommand>(command);
                mappedPerson.AddressId = addressId;
                var personIdResult = await _mediator.Send(mappedPerson);
                if (!personIdResult.Succeeded)
                {
                    return Result<int>.Fail(personIdResult.Messages);
                }
                int personId = personIdResult.Data;

                if (command.Id == 0)
                {
                    patient = _mapper.Map<Patient>(command);
                    patient.ClientId = _clientId;
                    patient.PersonId = personId;

                    await _patientRepository.InsertAsync(patient);
                    return Result<int>.Success(patient.Id, _localizer["Patient Saved"]);
                }
                else
                {
                    patient = await _patientRepository.GetByIdAsync(command.Id);
                    if (patient != null)
                    {
                        patient = _mapper.Map<Patient>(command);
                        patient.ClientId = _clientId;

                        await _patientRepository.UpdateAsync(patient);
                        await _unitOfWork.Commit(cancellationToken);

                        return Result<int>.Success(patient.Id, _localizer["Patient Updated"]);
                    }
                    else
                    {
                        return Result<int>.Fail(_localizer["Patient Not Found!"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(ex.Message);
            }
        }
    }
}