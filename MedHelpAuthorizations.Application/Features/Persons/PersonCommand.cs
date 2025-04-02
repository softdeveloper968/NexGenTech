using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Persons
{
    public abstract class PersonCommand : IRequest<Result<int>>
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string OfficePhoneNumber { get; set; }
        public string Email { get; set; }
        public int? AddressId { get; set; }
        public AddressTypeEnum AddressTypeId { get; set; }
        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }
        public string City { get; set; }
        public StateEnum StateId { get; set; } = StateEnum.UNK;
        public string PostalCode { get; set; }
        public bool Normalized { get; set; }
        public int DeliveryPointBarcode { get; set; }
        public GenderIdentityEnum? GenderIdentityId { get; set; }
        public AdministrativeGenderEnum? AdministrativeGenderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string SocialSecurityNumber { get; set; } //AA-218
        //public int ClientEntityId { get; set; }
    }

    public abstract class PersonCommandHandler<T> : IRequestHandler<T, Result<int>> where T : PersonCommand
    {
        public abstract Task<Result<int>> Handle(T personRequest, CancellationToken cancellationToken);
    }
}
