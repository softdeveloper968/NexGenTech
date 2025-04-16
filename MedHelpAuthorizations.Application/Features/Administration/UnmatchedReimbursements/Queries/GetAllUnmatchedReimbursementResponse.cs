namespace MedHelpAuthorizations.Application.Features.Administration.UnmatchedReimbursements.Queries
{
    public class GetAllUnmatchedReimbursementResponse
    {
        public int Id { get; set; }
        public string ProcedureCode { get; set; }
        public int ClientId { get; set; }
        public decimal TotalAllowedAmount { get; set; }
        public int ClientFeeScheduleEntryId { get; set; }
        public string PayerName { get; set; }
        public string FeeScheduleName { get; set; }
        public decimal Fee { get; set; }
        public int ClientFeeScheduleId { get; set; }
    }
}
