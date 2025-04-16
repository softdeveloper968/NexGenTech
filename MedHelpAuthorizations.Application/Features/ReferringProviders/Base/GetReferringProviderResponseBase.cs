
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.ReferringProviders.Queries.Base
{
    public class GetReferringProviderResponseBase
    {
        public int? Id { get; set; }
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Credentials { get; set; }
        public string Email { get; set; }
        public long? OfficePhoneNumber { get; set; }
        public long? FaxNumber { get; set; }
        public int? AddressId { get; set; } = null;
        //public Address? Address { get; set; } = null;
        public DateTime? DateOfBirth { get; set; } = null;
        public SpecialtyEnum SpecialtyId { get; set; }
        public string License { get; set; }
        public string Npi { get; set; }
        public string TaxId { get; set; }
        public string Upin { get; set; }
        public string TaxonomyCode { get; set; }
        public string ExternalId { get; set; }
        public int ClientId { get; set; }
        public int ClientEntityId { get; set; }
    }
}
