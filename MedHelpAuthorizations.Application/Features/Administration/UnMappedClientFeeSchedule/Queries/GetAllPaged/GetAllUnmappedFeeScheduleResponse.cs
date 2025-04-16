using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;

namespace MedHelpAuthorizations.Application.Features.Administration.UnMappedClientFeeSchedule.Queries.GetAllPaged
{
    public class GetAllUnmappedFeeScheduleResponse
    {
        public int Id { get; set; }
        public int ClientInsuranceId { get; set; }
        public string ProcedureCode { get; set; }
        public int ClientId { get; set; }
        public ClientInsuranceDto ClientInsurance { get; set; }
        public decimal BilledAmount { get; set; }
        public int DateOfServiceYear { get; set; }
        public DateTime ReferencedDateOfServiceFrom { get; set; }
        public decimal? LastReimbursement { get; set; }
        public ClientCptCodeDto ClientCptCode { get; set; }
    }
}
