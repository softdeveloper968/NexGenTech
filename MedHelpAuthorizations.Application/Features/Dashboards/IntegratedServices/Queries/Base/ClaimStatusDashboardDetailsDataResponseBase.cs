using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public class ClaimStatusDashboardDetailsResponseBase : IClaimStatusDashboardDetailsResponse
    {
        [ExcelCustom("PatientLastName", CustomTypeCode.String, "Last Name", "")]
        public string PatientLastName { get; set; }

        [ExcelCustom("PatientFirstName", CustomTypeCode.String, "First Name", "")]
        public string PatientFirstName { get; set; }

        [ExcelCustom("DateOfBirth", CustomTypeCode.String, "DOB", "")]
        public DateTime? DateOfBirth { get; set; }

        [ExcelCustom("PolicyNumber", CustomTypeCode.String, "Policy Number", "")]
        public string DateOfBirthString { get; set; } = string.Empty; //EN-66
        
        public string PolicyNumber { get; set; }

        [ExcelCustom("OfficeClaimNumber", CustomTypeCode.String, "Office Claim #", "")]
        public string OfficeClaimNumber { get; set; }

        [ExcelCustom("PayerName", CustomTypeCode.String, "Payer Name", "")]
        public string PayerName { get; set; }

        [ExcelCustom("PayerClaimNumber", CustomTypeCode.String, "Ins Claim #", "")]
        public string PayerClaimNumber { get; set; }

        [ExcelCustom("PayerLineItemControlNumber", CustomTypeCode.String, "Ins Lineitem Control #", "")]
        public string PayerLineItemControlNumber { get; set; }

        [ExcelCustom("DateOfServiceFrom", CustomTypeCode.String, "DOS From", "")]
        public DateTime? DateOfServiceFrom { get; set; }

        public string DateOfServiceFromString { get; set; } = string.Empty;        

        [ExcelCustom("DateOfServiceTo", CustomTypeCode.String, "DOS To", "")]
        public DateTime? DateOfServiceTo { get; set; }

        public string DateOfServiceToString { get; set; } = string.Empty;        

        [ExcelCustom("ProcedureCode", CustomTypeCode.String, "CPT Code", "")]
        public string ProcedureCode { get; set; }

        [ExcelCustom("Quantity", CustomTypeCode.String, "Quantity", "")]
        public int Quantity { get; set; } = 1;

        [ExcelCustom("ClaimBilledOn", CustomTypeCode.String, "Billed On", "")]
        public DateTime? ClaimBilledOn { get; set; }

        public string ClaimBilledOnString { get; set; } = string.Empty;
        
        [ExcelCustom("BilledAmount", CustomTypeCode.String, "Billed Amt", "")]
        public decimal BilledAmount { get; set; }

        public decimal? TotalAllowedAmount { get; set; }

        public decimal? NonAllowedAmount { get; set; }
        
        public string ServiceType { get; set; }
        
        public string ClaimLineItemStatus { get; set; }

        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; } //AA-317

        public string ClaimLineItemStatusValue { get; set; }

        public string ExceptionReasonCategory { get; set; }
        
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

        public string CheckDateString { get; set; } = string.Empty;
        
        public decimal? CheckPaidAmount { get; set; }
        
        public string EligibilityInsurance { get; set; }
        
        public string EligibilityPolicyNumber { get; set; }
        
        public DateTime? EligibilityFromDate { get; set; }
        public string EligibilityFromDateString { get; set; } = string.Empty;
        
        public string EligibilityStatus { get; set; }
        
        public string BatchNumber { get; set; }
        
        public string AitClaimReceivedDate { get; set; }
        
        public string AitClaimReceivedTime { get; set; }
        
        public string TransactionDate { get; set; }
        
        public string TransactionTime { get; set; }
        
        public string VerifiedMemberId { get; set; }
        
        public DateTime? CobLastVerified { get; set; }

        public string CobLastVerifiedString { get; set; } = string.Empty;
        
        public string LastActiveEligibleDateRange { get; set; }
        
        public string PrimaryPayer { get; set; }
        
        public string PrimaryPolicyNumber { get; set; }
        
        public DateTime? PartA_EligibilityFrom { get; set; }
        
        public DateTime? PartA_EligibilityTo { get; set; }
        
        public DateTime? PartA_DeductibleFrom { get; set; }
        
        public DateTime? PartA_DeductibleTo { get; set; }
        
        public decimal? PartA_RemainingDeductible { get; set; }
        
        public DateTime? PartB_EligibilityFrom { get; set; }
        
        public DateTime? PartB_EligibilityTo { get; set; }
        
        public DateTime? PartB_DeductibleFrom { get; set; }
        
        public DateTime? PartB_DeductibleTo { get; set; }
        
        public decimal? PartB_RemainingDeductible { get; set; }
        
        public DateTime? OtCapYearFrom { get; set; }
        
        public DateTime? OtCapYearTo { get; set; }
        
        public decimal? OtCapUsedAmount { get; set; }
        
        public DateTime? PtCapYearFrom { get; set; }
        
        public DateTime? PtCapYearTo { get; set; }
        
        public decimal? PtCapUsedAmount { get; set; }
        
        public string ClientProviderName { get; set; }

        public string ClientLocationName { get; set; }

        public string ClientLocationNpi { get; set; }

        public string PaymentType { get; set; } //AA-324

        //public DateTime? LastHistoryCreatedOn { get; set; }  //EN-127

		public int? ClaimStatusBatchClaimId { get; set; } //EN-127
	}
}
