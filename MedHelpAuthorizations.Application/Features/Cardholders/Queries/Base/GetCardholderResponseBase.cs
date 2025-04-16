using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.Cardholders.Queries.Base
{
    public class GetCardholderResponseBase
    {
        public int CardholderId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string CardholderLastName { get; set; }

        public string CardholderFirstName { get; set; }

        public string CardholderMiddleName { get; set; }

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
        
        public int PersonId { get; set; }

        public GenderIdentityEnum? GenderIdentityId { get; set; }

        public int? SocialSecurityNumber { get; set; }
    }
}
