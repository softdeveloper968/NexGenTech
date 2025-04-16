using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Commands.Base
{
    public class BaseClaimStatusTransactionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int? ClientId { get; set; }
        public int ClaimStatusBatchClaimId { get; set; }
        public DateTime ClaimStatusTransactionBeginDateTimeUtc { get; set; }
        public DateTime ClaimStatusTransactionEndDateTimeUtc { get; set; }
        public string LineItemControlNumber { get; set; }
        public ClaimStatusEnum? TotalClaimStatusId { get; set; }
        public string TotalClaimStatusValue { get; set; }
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
        public string ClaimLineItemStatusValue { get; set;}
        public int? ClaimStatusTransactionLineItemStatusChangẹId { get; set; }
        public ClaimStatusExceptionReasonCategoryEnum? ExceptionReasonCategoryId { get; set; }
        public string ExceptionReason { get; set; }
        public string ExceptionRemark { get; set; }
        public string RemarkCode { get; set; }
        public string RemarkDescription { get; set; }
        public string ReasonCode { get; set; }
        public string ReasonDescription { get; set; }
        public string X12ClaimStatusCode { get; set; }
        public string X12ClaimStatusCodeDescription { get; set; }
        public decimal? LineItemChargeAmount { get; set; }
        public decimal? TotalClaimChargeAmount { get; set; }
        public decimal? TotalNonAllowedAmount { get; set; }
        public decimal? TotalAllowedAmount { get; set; }
        public decimal? TotalMemberResponsibilityAmount { get; set; }
        public decimal? DeductibleAmount { get; set; }
        public decimal? CopayAmount { get; set; }
        public decimal? CoinsuranceAmount { get; set; }
        public decimal? CobAmount { get; set; }
        public decimal? PenalityAmount { get; set; }
        public decimal? LineItemPaidAmount { get; set; }
        public decimal? LineItemApprovedAmount { get; set; }
        public string ClaimNumber { get; set; }
        public string DiagnosisCode { get; set; }
        public DateTime? DateReceived { get; set; }
        public DateTime? DateEntered { get; set; }
        public string? ServiceLineDenialReason { get; set; }
        public DateTime? DatePaid { get; set; }
        public string? DiagnosisDescription { get; set; }
        public string CheckNumber { get; set; }
        public DateTime? CheckDate { get; set; }
        public decimal? CheckPaidAmount { get; set; }
        public bool? AuthorizationFound { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public AuthorizationStatusEnum? AuthorizationStatusId { get; set; }
        public string AuthorizationNumber { get; set; }
        public string EligibilityInsurance { get; set; }
        public string EligibilityPolicyNumber { get; set; }
        public DateTime? EligibilityFromDate { get; set; }
        public string LastActiveEligibleDateRange { get; set; }
        public string EligibilityStatus { get; set; }
        public string EligibilityPhone { get; set; } = string.Empty;
        public string EligibilityUrl { get; set; } = string.Empty;
        public int? InputDataListIndex { get; set; }
        public string InputDataFileName { get; set; }
        public decimal? TotalClaimPaidAmount { get; set; }
        public string VerifiedMemberId { get; set; }
        public DateTime? CobLastVerified { get; set; }
        public string PrimaryPayer { get; set; }
        public string PrimaryPolicyNumber { get; set; }
        public string Icn { get; set; }
        public string ReferringProviderName { get; set; }
        public string PlanType { get; set; }
        public string CurrentCoverage { get; set; }
        public string HippaStatus { get; set; }
        public string PaymentType { get; set; }
        public string BillingProviderNpi { get; set; }
        public DateTime? ClaimFinalizedOn { get; set; }
        public string CobaInsurerName { get; set; }
        public DateTime? CobaInsurerEffectiveDate { get; set; }
        public DateTime? PartA_EligibilityFrom { get; set; }
        public DateTime? PartA_EligibilityTo { get; set; }
        public DateTime? PartA_DeductibleFrom { get; set; }
        public DateTime? PartA_DeductibleTo { get; set; }
        public decimal? PartA_RemainingDeductible { get; set; }
        public DateTime? PartB_EligibilityFrom { get; set; }
        public DateTime? PartBEligibilityTo{ get; set; }
        public DateTime? PartB_DeductibleFrom { get; set; }
        public DateTime? PartBDeductibleTo { get; set; }
        public decimal? PartB_RemainingDeductible { get; set; }
        public DateTime? OtCapYearFrom { get; set; }
        public DateTime? OtCapYearTo { get; set; }
        public decimal? OtCapUsedAmount { get; set; }
        public DateTime? PtCapYearFrom { get; set; }
        public DateTime? PtCapYearTo { get; set; }
        public decimal? PtCapUsedAmount { get; set; }
        public string ErrorMessage { get; set; }    
		public string CurlScript { get; set; }
	}
}
