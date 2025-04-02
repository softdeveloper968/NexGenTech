using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Addresses.Commands.Base
{
    public abstract class AddressCommand : IRequest<Result<int>>
    {
        public int AddressId { get; set; }
        public string ExternalId { get; set; }
        public AddressTypeEnum AddressTypeId { get; set; } = AddressTypeEnum.Residential;
        public string AddressStreetLine1 { get; set; }
        public string AddressStreetLine2 { get; set; }
        public string City { get; set; }
        public StateEnum StateId { get; set; }
        public string PostalCode { get; set; }
        public bool Normalized { get; set; }
        public int DeliveryPointBarcode { get; set; }
        //public int ClientId { get; set; }
        //public int ClientEntityId { get; set; }
    }  
}
