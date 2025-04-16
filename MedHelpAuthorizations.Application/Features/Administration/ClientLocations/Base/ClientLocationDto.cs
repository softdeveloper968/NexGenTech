using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base
{
    public class ClientLocationDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        
        public long? OfficePhoneNumber { get; set; } = null;

        public long? OfficeFaxNumber { get; set; } = default(long?);

        public int? AddressId { get; set; } = default(int?);
        public int? EligibilityLocationId { get; set; } = default(int?);

        [StringLength(10)]
        public string Npi { get; set; }


        [StringLength(36)]
        public string ExternalId { get; set; }

    }
}
