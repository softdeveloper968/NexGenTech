using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientPlacesOfService.Queries.GetAllPaged
{
    public class GetAllClientPlacesOfServiceResponse
    {
        public string Name { get; set; }
        public string LookupName { get; set; }
        public long? OfficePhoneNumber { get; set; }
        public long? OfficeFaxNumber { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string Npi { get; set; }
        public PlaceOfServiceCodeEnum PlaceOfServiceCodeId { get; set; }
        public int ClientId { get; set; }
    }
}
