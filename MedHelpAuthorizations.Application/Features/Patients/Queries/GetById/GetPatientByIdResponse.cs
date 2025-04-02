using System;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Patients.Queries.GetById
{
    public class GetPatientByIdResponse
    {
        public int Id { get; set; }

        public int PersonId { get; set; }

        public string AccountNumber { get; set; }

        public string ExternalId { get; set; }

        public string LastName { get; set; }

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

        public DateTime? DateOfBirth { get; set; }

        public AdministrativeGenderEnum AdministrativeGenderId { get; set; }

        public GenderIdentityEnum GenderIdentityId { get; set; }

        public string SocialSecurityNumber { get; set; }
        public string DecryptedSocialSecurityNumber //AA-218
        {
            get
            {
                return string.IsNullOrEmpty(SocialSecurityNumber) ? string.Empty : SocialSecurityNumberExtensions.DecryptSSN(SocialSecurityNumber);
            }
            set
            {
                SocialSecurityNumber = string.IsNullOrEmpty(value) ? string.Empty : SocialSecurityNumberExtensions.EncryptSSN(value);
            }
        }

        //public string Insurance { get; set; }

        //public int? ClientInsuranceId { get; set; }

        //public string InsurancePolicyNumber { get; set; }

        //public string InsuranceGroupNumber { get; set; }

        public int ClientId { get; set; }

        public RelationShipTypeEnum ResponsiblePartyRelationshipToPatient { get; set; }

        public int? ResponsiblePartyId { get; set; }

        public int? PrimaryProviderId { get; set; }

        public int? ReferringProviderId { get; set; }

        public DateTime? BenefitsCheckedOn { get; set; }

        //public List<InsuranceCard> InsuranceCards { get; set; } = new List<InsuranceCard>();
    }
}