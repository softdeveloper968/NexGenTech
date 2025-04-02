using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base
{
    public class ClientInsuranceDto 
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        [StringLength(125)]
        public string LookupName { get; set; }

        [StringLength(125)]
        public string Name { get; set; }
        //public int? InsuranceCategoryId { get; set; }
        public long? PhoneNumber { get; set; } = default(long?);
        public long? FaxNumber { get; set; } = default(long?);

        //[StringLength(14)]
        //public string OfficePhoneNumber { get; set; };

        [StringLength(30)]
        public string ExternalId { get; set; }

        [StringLength(30)]
        public string PayerIdentifier { get; set; }

        public int? RpaInsuranceId { get; set; } = default(int?);
    }
}
