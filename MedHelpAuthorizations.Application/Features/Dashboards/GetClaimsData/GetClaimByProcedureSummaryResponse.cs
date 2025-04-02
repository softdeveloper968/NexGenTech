namespace MedHelpAuthorizations.Application.Features.Dashboards.GetClaimsData
{
    public class GetClaimByProcedureSummaryResponse
    {
        public int? ClientId { get; set; }
        public int? ClaimLineItemStatusId { get; set; }
        public string ClaimStatusExceptionReasonCategoryId { get; set; }
        public int? ClientCptCodeId { get; set; }
        public int Quantity { get; set; }
        public decimal ChargedSum { get; set; }
        public decimal PaidAmountSum { get; set; }
        public decimal AllowedAmountSum { get; set; }
        public decimal NonAllowedAmountSum { get; set; }
        public int? ClientLocationId { get; set; }
        public int? ClientProviderId { get; set; }
        public decimal WriteOffAmountSum { get; set; }
        public DateTime BatchProcessDate { get; set; }
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? ClaimReceivedDate { get; set; }
        public DateTime? BilledOnDate { get;set;}
        public int ClientInsuranceId { get; set; }
    }
}
