using MedHelpAuthorizations.Application.Features.Addresses.ViewModels;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.Persons.ViewModels
{
    public class PersonDto
    {
        public int id { get; set; } = 0;

        public int PersonId { get; set; } = 0;
        
        public string FirstName { get; set; }

        public string MiddleName { get; set; }
        
        public string LastName { get; set; }

        public string LastCommaFirstName => $"{LastName}, {FirstName}";

        public string FirstInitialLastName => $"{FirstName?.Substring(0, 1)}. {LastName}";

        public string FirstMiddleLastName => $"{FirstName} {MiddleName} {LastName}";

        public int? SocialSecurityNumber { get; set; }

        public long? HomePhoneNumber { get; set; }

        public long? MobilePhoneNumber { get; set; }

        public long? OfficePhoneNumber { get; set; }

        public string Email { get; set; }

        public int? AddressId { get; set; } = null;

        public AddressDto Address { get; set; } = new AddressDto();

		public GenderIdentityEnum? GenderIdentityId { get; set; } = null;
        
        public AdministrativeGenderEnum? AdministrativeGenderId { get; set; } = null;
        
        public DateTime? DateOfBirth { get; set; } = null;
    }
}
