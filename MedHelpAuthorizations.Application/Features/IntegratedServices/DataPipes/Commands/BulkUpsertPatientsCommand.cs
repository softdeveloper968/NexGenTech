using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.DataPipes.Commands
{
    public class BulkUpsertPatientsCommand : IRequest<Result<int>>
    {
        //public int Id { get; set; }

        public int External { get; set; }

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

        public int? SocialSecurityNumber { get; set; }

        public RelationShipTypeEnum ResponsiblePartyRelationshipToPatient { get; set; }

        public int? ResponsiblePartyId { get; set; }

        public int? PrimaryProviderId { get; set; }

        public int? ReferringProviderId { get; set; }

        public class BulkInsertPatientsCommandHandler : IRequestHandler<BulkUpsertPatientsCommand, Result<int>>
        {
            private readonly IMapper _mapper;
            private readonly IPatientRepository _patientRepository;
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IUploadService _uploadService;
            private readonly IStringLocalizer<BulkInsertPatientsCommandHandler> _localizer;
            private readonly IMediator _mediator;
            private readonly ICurrentUserService _currentUserService;
            private int _clientId => _currentUserService.ClientId;

            public BulkInsertPatientsCommandHandler(IPatientRepository patientRepository, IUnitOfWork<int> unitOfWork,
                                                IMapper mapper, IUploadService uploadService,
                                                IStringLocalizer<BulkInsertPatientsCommandHandler> localizer,
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

            public async Task<Result<int>> Handle(BulkUpsertPatientsCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    return null;

                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(ex.Message);
                }
            }
        }
    }
}
