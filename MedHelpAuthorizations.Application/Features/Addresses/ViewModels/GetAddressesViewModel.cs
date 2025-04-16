using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Addresses.ViewModels
{
    public class GetAddressesViewModel
    {
        public int AddressId { get; set; }           
        public string ExternalId { get; set; }
        public AddressTypeEnum AddressTypeId { get; set; }
        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public bool Normalized { get; set; }
        public int DeliveryPointBarcode { get; set; }
    }
}
