using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Addresses.ViewModels
{
	public class AddressDto
	{
		public int Id { get; set; }
		public AddressTypeEnum AddressTypeId { get; set; } = AddressTypeEnum.Residential;
		public string AddressStreetLine1 { get; set; }
		public string AddressStreetLine2 { get; set; }
		public string City { get; set; }
		public StateEnum StateId { get; set; } = StateEnum.UNK;
		public string PostalCode { get; set; }
		public bool Normalized { get; set; } = false;
		public int DeliveryPointBarcode { get; set; }
		public int ClientId { get; set; } 
	}
}
