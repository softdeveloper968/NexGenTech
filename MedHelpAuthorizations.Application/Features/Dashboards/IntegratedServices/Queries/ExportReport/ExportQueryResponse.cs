using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetInputDocumentMessageById;
using MedHelpAuthorizations.Application.Features.Reports.Queries.Export;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport
{
    public class ExportQueryResponse : ExportSummaryReportResponse
    {

        #region Common Export columns.

        public string PatientLastName { get; set; }

        public string PatientFirstName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthString { get; set; } = string.Empty;

        public string PolicyNumber { get; set; }

        public string OfficeClaimNumber { get; set; }

        public string PayerName { get; set; }

        public string PayerClaimNumber { get; set; }

        public string PayerLineItemControlNumber { get; set; }

        public DateTime? DateOfServiceFrom { get; set; }

        public string DateOfServiceFromString { get; set; } = string.Empty;

        public DateTime? DateOfServiceTo { get; set; }

        public string DateOfServiceToString { get; set; } = string.Empty;

        public string ProcedureCode { get; set; }

        public int Quantity { get; set; } = 1;

        public DateTime? ClaimBilledOn { get; set; }

        public string ClaimBilledOnString { get; set; } = string.Empty;

        public decimal BilledAmount { get; set; }

        public decimal? TotalAllowedAmount { get; set; }

        public decimal? NonAllowedAmount { get; set; }

        public string ServiceType { get; set; }

        public string ClaimLineItemStatus { get; set; }

        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }

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

        public string PaymentType { get; set; }

        public DateTime? LastHistoryCreatedOn { get; set; }

        public int? ClaimStatusBatchClaimId { get; set; }

        #endregion

        #region AR Aging Export Report

        public List<ARAgingData> ARAgingData { get; set; }

        // ChargedSum
        public decimal AgeGroup_0_30 { get; set; } = 0.0m;
        public decimal AgeGroup_31_60 { get; set; } = 0.0m;
        public decimal AgeGroup_61_90 { get; set; } = 0.0m;
        public decimal AgeGroup_91_120 { get; set; } = 0.0m;
        public decimal AgeGroup_121_150 { get; set; } = 0.0m;
        public decimal AgeGroup_151_180 { get; set; } = 0.0m;
        public decimal AgeGroup_181_plus { get; set; } = 0.0m;

        // Quantity
        public int AgeGroup_0_30_Qty { get; set; } = 0;
        public int AgeGroup_31_60_Qty { get; set; } = 0;
        public int AgeGroup_61_90_Qty { get; set; } = 0;
        public int AgeGroup_91_120_Qty { get; set; } = 0;
        public int AgeGroup_121_150_Qty { get; set; } = 0;
        public int AgeGroup_151_180_Qty { get; set; } = 0;
        public int AgeGroup_181_plus_Qty { get; set; } = 0;

        #endregion

        #region CLaim Status Revenue Total

        public string ClientInsuranceName { get; set; }
        public string ClaimStatusExceptionReasonCategory { get; set; }
        public decimal ChargedSum { get; set; }
        public decimal PaidAmountSum { get; set; }
        public decimal AllowedAmountSum { get; set; }
        public decimal NonAllowedAmountSum { get; set; }
        public string EntryHash { get; set; }
        public decimal WriteOffAmountSum { get; set; }

        #endregion

        #region GetCashProjectionByDayResponse

        /// <summary>
        /// Gets or sets the count of claims.
        /// </summary>
        public int ClaimCount { get; set; } = 0;
        public bool FailureReported { get; set; } = false;
        public bool IsExpiryWarningReported { get; set; } = false;

        /// <summary>
        /// Gets or sets the total paid amounts.
        /// </summary>
        public decimal PaidTotals { get; set; } = 0.00m;

        /// <summary>
        /// Gets or sets the total revenue amounts.
        /// </summary>
        public decimal RevenueTotals { get; set; } = 0.00m;

        /// <summary>
        /// Gets or sets the MD5 hash associated with the claim.
        /// </summary>
        public string ClaimLevelMd5Hash { get; set; }

        /// <summary>
        /// Gets or sets the ID of the client's insurance.
        /// </summary>
        public int ClientInsuranceId { get; set; }

        /// <summary>
        /// Gets or sets the account number.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the external identifier.
        /// </summary>
        public string ExternalId { get; set; }

        /// <summary>
        /// Gets or sets the concatenated name of the patient in "Last, First" format.
        /// </summary>
        public string PatientLastCommaFirst { get; set; }

        /// <summary>
        /// Line Item Charge-Amount
        /// </summary>
        public decimal? LineItemChargeAmount { get; set; }

        public string LastCheckedDate { get; set; } = string.Empty;
        public string LastCheckedTime { get; set; } = string.Empty;

        #endregion

        #region ARAgingData
        public string LocationName { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public DateTime MyProperty { get; set; }
        public string InsuranceTitle { get; set; }
        public int InsuranceId { get; set; }
        public int? LocationId { get; set; }
        public int? ProviderId { get; set; }
        public decimal InsuranceTotal { get; set; } = 0.0m;
        public decimal TotalInsurancePercent { get; set; } = 0.0m;

        #endregion

        #region AllPagedAuthorizations

        public int PagedAuthorizationId { get; set; }
        public int AuthTypeId { get; set; }
        public string AuthTypeName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientAccountNumber { get; set; }
        public DateTime? PatientDateOfBirth { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Completeby { get; set; }
        public string AuthNumber { get; set; }
        public int? Units { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DischargedOn { get; set; }
        public string DischargedBy { get; set; }
        public string CreateUserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ClientId { get; set; }
        public ICollection<AuthorizationClientCptCode> AuthorizationClientCptCodes { get; set; } = new HashSet<AuthorizationClientCptCode>();
        public AuthorizationStatusEnum AuthorizationStatusId { get; set; }
        public IList<Document> Documents { get; set; } = new List<Document>();
        public IList<string> NeededDocumentTypes { get; set; } = new List<string>();
        public bool HasDocuments => Documents.Any();
        public bool NeedsDocuments => NeededDocumentTypes.Any();

        #endregion

        #region ClaimStatusDateLagResponse

        public string ClaimNumber { get; set; }
        public int ServiceToBilledDateLag { get; set; } = 0;
        public int ServiceToPaymentDateLag { get; set; } = 0;
        public int BilledToPaymentDateLag { get; set; } = 0;
        public decimal AvgServiceToBilledDateLag { get; set; } = 0.00m;
        public decimal AvgServiceToPaymentDateLag { get; set; } = 0.00m;
        public decimal AvgBilledToPaymentDateLag { get; set; } = 0.00m;

        #endregion


        #region CashValueForRevenueByDayResponse

        /// <summary>
        /// Gets or sets the cash value.
        /// </summary>
        public decimal CashValue { get; set; } = 0.00m;
        /// <summary>
        /// Gets or sets the service date.
        /// </summary>
        public string ServiceDate { get; set; }
        /// <summary>
        /// Gets or sets the billed date.
        /// </summary>
        public string BilledDate { get; set; }

        /// <summary>
        /// Location associated
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Provider associated
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// Estimated payment value
        /// </summary>
        public decimal EstimatedPayment { get; set; }

        #endregion

        #region Audit

        public int AuditId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }

        #endregion

        #region ClaimStatusTotalResultExceptionModel

        public string ClientName { get; set; }
        public string ExceptionMessage { get; set; }


        #endregion

        #region ClaimStatusDaysWaitLapsedDetailResponse

        public int? ClaimStatusTransactionId { get; set; }
        public int? ClaimStatusTransactionLineItemStatusChangẹId { get; set; }
        public string StatusLastCheckedOn { get; set; }
        public string DaysLapsed { get; set; }

        public int ClaimStatusBatchId { get; set; }

        #endregion

        #region DailyClaimStatusReportResponse
        public DateTime ClaimBilledDate { get; set; }
        public int Reviewed { get; set; }
        public decimal ReviewedPercentage { get; set; }
        public int InProcess { get; set; }
        public decimal InProcessPercentage { get; set; }
        public int ApprovedPaid { get; set; }
        public decimal ApprovePaidPercentage { get; set; }
        public int Denied { get; set; }
        public decimal DeniedPercentage { get; set; }
        public int NotAdjudicated { get; set; }
        public decimal NotAdjudicatedPercentage { get; set; }
        public int ZeroPaid { get; set; }
        public decimal ZeroPaidPercentage { get; set; }
        /// <summary>
        /// Total Claim Received or Total Uploaded claims.
        /// </summary>
        public int ClaimReceived { get; set; }
        public string ClaimDate { get; set; }

        #endregion

        #region FailedClaimStatusBatchesExcelData

        public string ClientInsurance_ClientCode { get; set; } // Assuming this is a string
        public string AssignedDateTimeUtc { get; set; } // Using string since you convert DateTime to ShortDateString
        public string AssignedToRpaCode { get; set; } // Assuming this is a string
        public string CompletedDateTimeUtc { get; set; } // Using string for date that's been converted to short date string
        public string AbortedOnUtc { get; set; } // Using string for date that's been converted to short date string
        public string AbortedReason { get; set; } // Assuming this is a string
        public string CreatedOnString { get; set; } // Using string since you convert DateTime to ShortDateString
        public string LastModifiedOn { get; set; } // Using string for date that's been converted to short date string
        public int? AssignedClientRpaConfigurationId { get; set; } // Assuming this is an integer


        #endregion

        #region Failed Insurance Data

        public int ConfigurationId { get; set; }
        public string ClientCode { get; set; }
        public string ClientInsuranceLookupName { get; set; }
        public string ReportFailureToEmail { get; set; }
        public string ExpiryWarningReported { get; set; }
        public object Username { get; set; }
        public object Password { get; set; }
        public object TargetUrl { get; set; }
        public object FailureMessage { get; set; }

        #endregion

        #region ApprovedClaimsDetailResponse

        public string InsuranceName { get; set; }
        public string InsuranceClaimNumber { get; set; }
        public DateTime? ApprovedSinceDate { get; set; }
        public DateTime? DateOfService { get; set; }
        public string DateOfServiceString { get; set; }
        public string Modifiers { get; set; }

        #endregion

        #region EmployeesClaimStatusResponseModel

        public int? ClientLocationId { get; set; }
        public ClaimLineItemStatusEnum? PreviousClaimLineItemStatusId { get; set; }
        public ClaimLineItemStatusEnum? UpdatedClaimLineItemStatusId { get; set; }
        public ClaimStatusExceptionReasonCategoryEnum ClaimStatusExceptionReasonCategorId { get; set; }

        #endregion

        #region CheckStatusPercentageThresholdResponse

        public int BatchId { get; set; }
        public string RpaInsuranceCode { get; set; }
        public string BatchClaimCount { get; set; }
        public int UnavailableCount { get; set; }
        public int NotFoundCount { get; set; }
        public string UnavailablePercentage { get; set; }
        public string NotFoundPercentage { get; set; }
        public int CountNotFoundUnavailable { get; set; }
        public string PercentNotFoundUnavailable { get; set; }
        public double FilterValue { get; set; }
        public string ClaimStatusTypeId { get; set; }

        #endregion

        #region InputDocumentMessages  En-496
        public int Id { get; set; }
        public int InputDocumentId { get; set; }
        public InputDocumentMessageTypeEnum MessageType { get; set; }
        public string Message { get; set; }

        // Properties to store message information for each type
        public List<MessageInfoViewModel> ErroredMessages { get; set; }
        public List<MessageInfoViewModel> UnmatchedLocationMessages { get; set; }
        public List<MessageInfoViewModel> UnmatchedProviderMessages { get; set; }
        #endregion

        #region Avg. Days To Pay Report

        /// <summary>
        /// Used In Avg. Days To Pay Summary
        /// </summary>
        public double AverageDaysToPay { get; set; }
        /// <summary>
        /// Used In Avg. Days To Pay Summary
        /// </summary>
        public double AverageDaysToBill { get; set; }
        public int AvgDaysToPay { get; set; }
        public int AvgDaysToBill { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? DOB { get; set; }
        public string InsClaimNumber { get; set; }
        public string CptCode { get; set; }
        public string LineitemStatus { get; set; }
        public decimal LineitemPaidAmount { get; set; }
        public decimal AllowedAmount { get; set; }
        public string AitBatchNumber { get; set; }
        public DateTime? AitReceivedDate { get; set; }
        public string AitReceivedTime { get; set; }
        public DateTime? AitTransactionDate { get; set; }
        public string AitTransactionTime { get; set; }
        public string ProviderNPI { get; set; }
        public int AvgDaysfromDOStoPay { get; set; }
        public double AverageDaysFromDosTOPay { get; set; }
        #endregion

        #region Finical Summary Sheet
        public string RowLabels { get; set; }
        public int LineItemStatusCount { get; set; }
        public decimal BilledAmountSum { get; set; }
        public decimal LineitemPaidAmtSum { get; set; }
        #endregion

        #region Procedure Code Summary 
        public int CptCodeCount { get; set; }
        public decimal AvgAllowedAmount { get; set; }
        public int PayerNameCount { get; set; }
        public decimal? DeductibleAmtSum { get;set; }
        public decimal? CopayAmtSum { get; set; }
        public decimal? CoInsuranceAmtSum { get; set; }
        public decimal? PenalityAmtSum { get; set; }
        public decimal? AvgLineitemPaidAmt { get; set; }
        public string AllowedToPaidPercentage { get => this.TotalAllowedAmount.HasValue ? ((TotalAllowedAmount/BilledAmount)*100)?.ToString("P") : "0.00%"; }
        #endregion

    }


}
