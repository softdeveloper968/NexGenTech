using System;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public interface IClaimStatusDashboardDetailsResponse
    {
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PolicyNumber { get; set; }
        public string OfficeClaimNumber { get; set; }
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public string ProcedureCode { get; set; }
        public int Quantity { get; set; }
        public DateTime? ClaimBilledOn { get; set; }
        public decimal BilledAmount { get; set; }
        public string ClaimLineItemStatus { get; set; }
        public string ClaimLineItemStatusValue { get; set; }
        public string ExceptionReason { get; set; }
        public string ExceptionRemark { get; set; }
        public string RemarkCode { get; set; }
        public string RemarkDescription { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDescription { get; set; }
        public decimal? DeductibleAmount { get; set; }
        public decimal? CopayAmount { get; set; }
        public decimal? CoinsuranceAmount { get; set; }
        public decimal? CobAmount { get; set; }
        public decimal? PenalityAmount { get; set; }
        public decimal? LineItemPaidAmount { get; set; }
        public decimal? LineItemApprovedAmount { get; set; }
        public string CheckNumber { get; set; }
        public DateTime? CheckDate { get; set; }
        public decimal? CheckPaidAmount { get; set; }
        public string EligibilityInsurance { get; set; }
        public string EligibilityPolicyNumber { get; set; }
        public DateTime? EligibilityFromDate { get; set; }
        public string EligibilityStatus { get; set; }
    }
}
