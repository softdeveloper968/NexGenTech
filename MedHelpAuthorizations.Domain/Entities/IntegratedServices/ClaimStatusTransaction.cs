using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    [CustomReportTypeEntityHeader(CustomReportHelper._ClaimStatusTransaction, CustomTypeCode.Empty, hasSubEntityExist: false)]
    public class ClaimStatusTransaction : AuditableEntity<int>
    {
        public ClaimStatusTransaction()
        {
            ClaimStatusTransactionHistories = new HashSet<ClaimStatusTransactionHistory>();
        }

        public ClaimStatusTransaction(int clientId, int claimStatusBatchClaimId, DateTime claimStatusTransactionBeginDateTimeUtc, DateTime claimStatusTransactionEndDateTimeUtc, ClaimStatusEnum totalClaimStatusId)
        {
            ClientId = clientId;
            ClaimStatusBatchClaimId = claimStatusBatchClaimId;
            ClaimStatusTransactionBeginDateTimeUtc = claimStatusTransactionBeginDateTimeUtc;
            ClaimStatusTransactionEndDateTimeUtc = claimStatusTransactionEndDateTimeUtc;
            TotalClaimStatusId = totalClaimStatusId;
            #region Navigational Property Init

            ClaimStatusTransactionHistories = new HashSet<ClaimStatusTransactionHistory>();
            ClaimStatusWorkstationNotes = new HashSet<ClaimStatusWorkstationNotes>();

            #endregion
        }

        public int? ClientId { get; set; }

        [Required]
        public int ClaimStatusBatchClaimId { get; set; }

        [Required]
        public DateTime ClaimStatusTransactionBeginDateTimeUtc { get; set; }

        [Required]
        public DateTime ClaimStatusTransactionEndDateTimeUtc { get; set; }

        [StringLength(25)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.LineItemControlNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string LineItemControlNumber { get; set; }

        public ClaimStatusEnum TotalClaimStatusId { get; set; } = ClaimStatusEnum.None;

        public string TotalClaimStatusValue { get; set; }

        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }// = ClaimLineItemStatusEnum.Unknown;

        public int? ClaimStatusTransactionLineItemStatusChangẹId { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ClaimLineItemStatusValue, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ClaimLineItemStatusValue { get; set; }

        //[CustomReportTypeColumnsHeaderForMainEntity]
        public string VerifiedMemberId { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity<ClaimStatusExceptionReasonCategoryEnum>(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.EnumType, propertyName: CustomReportHelper.ClaimStatusExceptionReasonCategoryId, hasPropertyEnumType: true)]
        public ClaimStatusExceptionReasonCategoryEnum? ClaimStatusExceptionReasonCategoryId { get; set; } = ClaimStatusExceptionReasonCategoryEnum.Other;

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ExceptionReason, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ExceptionReason { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ExceptionRemark, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ExceptionRemark { get; set; }

        [StringLength(72)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.RemarkCode, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string RemarkCode { get; set; }

        [StringLength(72)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ReasonCode, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ReasonCode { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ReasonDescription, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ReasonDescription { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.RemarkDescription, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string RemarkDescription { get; set; }

        public decimal? TotalClaimChargeAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.TotalNonAllowedAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? TotalNonAllowedAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.TotalAllowedAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? TotalAllowedAmount { get; set; }

        public decimal? TotalMemberResponsibilityAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.DeductibleAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? DeductibleAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.CopayAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? CopayAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.CoinsuranceAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? CoinsuranceAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.CobAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? CobAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.PenalityAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? PenalityAmount { get; set; }

        public decimal? LineItemChargeAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.LineItemPaidAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? LineItemPaidAmount { get; set; }

        [StringLength(25)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.ClaimNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string ClaimNumber { get; set; }

        [StringLength(64)]
        public string DiagnosisCode { get; set; }

        public string? ServiceLineDenialReason { get; set; }

        public DateTime? DateReceived { get; set; } = null;

        public DateTime? DateEntered { get; set; } = null;

        public string? DiagnosisDescription { get; set; }

        public DateTime? DatePaid { get; set; } = null;

        [StringLength(25)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.CheckNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string CheckNumber { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.DateRangeType, propertyName: CustomReportHelper.CheckDate, hasCustomDateRange: true, hasRelativeDateRange: true, hasDateRangeCombined: false)]
        public DateTime? CheckDate { get; set; } = null;

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.CheckPaidAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? CheckPaidAmount { get; set; }

        public bool? AuthorizationFound { get; set; }

        public bool IsDeleted { get; set; } = false;

        public AuthorizationStatusEnum? AuthorizationStatusId { get; set; }

        [StringLength(25)]
        public string AuthorizationNumber { get; set; } = string.Empty;

        [StringLength(50)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.EligibilityInsurance, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string EligibilityInsurance { get; set; } = string.Empty;

        [StringLength(25)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.EligibilityPolicyNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string EligibilityPolicyNumber { get; set; } = string.Empty;

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.DateRangeType, propertyName: CustomReportHelper.EligibilityFromDate, hasCustomDateRange: true, hasRelativeDateRange: true, hasDateRangeCombined: false)]
        public DateTime? EligibilityFromDate { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.LastActiveEligibleDateRange, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string LastActiveEligibleDateRange { get; set; }

        public string EligibilityPhone { get; set; } = string.Empty;

        public string EligibilityUrl { get; set; } = string.Empty;

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.CobLastVerified, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public DateTime? CobLastVerified { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.PrimaryPayer, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string PrimaryPayer { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.PrimaryPolicyNumber, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string PrimaryPolicyNumber { get; set; }

        [StringLength(16)]
        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.String, propertyName: CustomReportHelper.EligibilityStatus, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public string EligibilityStatus { get; set; } = string.Empty;

        public string Icn { get; set; }

        public string ReferringProviderName { get; set; }
        public string PlanType { get; set; }
        public string CurrentCoverage { get; set; }
        public string HippaStatus { get; set; }
        public string PaymentType { get; set; }

        public string BillingProviderNpi { get; set; }

        public DateTime? DateClaimFinalized { get; set; }

        public string CobaInsurerName { get; set; }

        public DateTime? CobaInsurerEffective { get; set; }

        public DateTime? PartA_EligibilityFrom { get; set; }

        public DateTime? PartA_EligibilityTo { get; set; }

        public DateTime? PartA_DeductibleFrom { get; set; }

        public DateTime? PartA_DeductibleToDate { get; set; }

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

        public int? InputDataListIndex { get; set; }

        public string InputDataFileName { get; set; }

        public decimal? TotalClaimPaidAmount { get; set; }

        [CustomReportTypeColumnsHeaderForMainEntity(entityName: CustomReportHelper._ClaimStatusTransaction, typeCode: CustomTypeCode.Decimal, propertyName: CustomReportHelper.WriteoffAmount, hasCustomDateRange: false, hasRelativeDateRange: false, hasDateRangeCombined: false)]
        public decimal? WriteoffAmount { get; set; }

        public string ErrorMessage { get; set; }

        public string CurlScript { get; set; }


        #region Navigational Property Access

        public virtual ICollection<ClaimStatusWorkstationNotes> ClaimStatusWorkstationNotes { get; set; }

        public virtual ICollection<ClaimStatusTransactionHistory> ClaimStatusTransactionHistories { get; set; }


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ClaimStatusTransactionLineItemStatusChangẹId")]
        public virtual ClaimStatusTransactionLineItemStatusChangẹ ClaimStatusTransactionLineItemStatusChangẹ { get; set; }

        [ForeignKey("ClaimStatusExceptionReasonCategoryId")]
        public virtual ClaimStatusExceptionReasonCategory ClaimStatusExceptionReasonCategory { get; set; }


        [ForeignKey("ClaimStatusBatchClaimId")]
        public virtual ClaimStatusBatchClaim ClaimStatusBatchClaim { get; set; }


        [ForeignKey("TotalClaimStatusId")]
        public virtual ClaimStatus TotalClaimStatus { get; set; }


        [ForeignKey("ClaimLineItemStatusId")]
        public virtual ClaimLineItemStatus ClaimLineItemStatus { get; set; }


        [ForeignKey("AuthorizationStatusId")]
        public virtual AuthorizationStatus AuthorizationStatus { get; set; }

        //[ForeignKey("WriteOffTypeId")]
        //public virtual WriteOffType WriteOffType { get; set; }

        #endregion
    }
}
