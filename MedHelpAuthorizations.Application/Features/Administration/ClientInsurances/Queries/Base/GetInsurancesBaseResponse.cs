using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetAllPaged
{
    public class GetClientInsurancesBaseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LookupName { get; set; }
        public long? PhoneNumber { get; set; }
        public long? FaxNumber { get; set; }
        public string ExternalId { get; set; }
        public string PayerIdentifier { get; set; }
        public int ClientId { get; set; }
        //public int? RpaInsuranceId { get; set; }
        public string RpaInsuranceCode { get; set; }
        public Domain.Entities.ClientFeeSchedule ClientFeeSchedule { get; set; }
        public int ClientFeeScheduleId { get; set; }
        public ICollection<Domain.Entities.ClientInsuranceFeeSchedule> ClientInsuranceFeeSchedules { get; set; }
        public bool RequireLocationInput { get; set; }
        public bool AutoCalcPenalty { get; set; }

    }
}
