using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ResponsibleParties.GetByCriteria
{
    public class GetByCritieriaResponsiblePartyResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int Age { get; set; }
        public long? HomePhoneNumber { get; set; } = null;

        public long? MobilePhoneNumber { get; set; } = null;
        public long? OfficePhoneNumber { get; set; } = null;
        public long? FaxNumber { get; set; } = null;
        public string Email { get; set; }

        public GenderIdentityEnum? GenderIdentityId { get; set; }


        public AdministrativeGenderEnum? AdministrativeGenderId { get; set; }

        public string AccountNumber { get; private set; }
        public string ExternalId { get; set; }
        public decimal? SocialSecurityNumber { get; set; }


        public AddressTypeEnum AddressTypeId { get; set; }

        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }

        public string City { get; set; }
        public StateEnum StateId { get; set; }
        public string PostalCode { get; set; }
        public int DeliveryPointBarcode { get; set; }


    }
}
