using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.Base
{
    public class GetClientLocationsBaseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long? OfficePhoneNumber { get; set; }
        public long? OfficeFaxNumber { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string Npi { get; set; }
        public int ClientId { get; set; }
        public int? EligibilityLocationId { get; set; }
    }
}
