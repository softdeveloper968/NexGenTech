using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ClaimStatusTransactionHistory : AuditableEntity<int>//, ITenant
    {
        public ClaimStatusTransactionHistory()
        {

        }

        public ClaimStatusTransactionHistory(int clientId, int claimStatusTransactionId, DateTime claimStatusTransactionBeginDateTimeUtc, DateTime claimStatusTransactionEndDateTimeUtc, ClaimStatusEnum totalClaimStatusId, DbOperationEnum dbOperationId)
        {
            this.ClaimStatusTransactionId = claimStatusTransactionId;
            this.ClaimStatusTransactionBeginDateTimeUtc = claimStatusTransactionBeginDateTimeUtc;
            this.ClaimStatusTransactionEndDateTimeUtc = claimStatusTransactionEndDateTimeUtc;
            this.TotalClaimStatusId = totalClaimStatusId;
            this.DbOperationId = dbOperationId;

            #region Navigational Property Init

            #endregion
        }

         public int? ClientId { get; set; }

        [Required]
        public int ClaimStatusTransactionId { get; set; }


        [Required]
        public DateTime ClaimStatusTransactionBeginDateTimeUtc { get; set; }


        [Required]
        public DateTime ClaimStatusTransactionEndDateTimeUtc { get; set; }


        [StringLength(25)]
        public string LineItemControlNumber { get; set; }

        public ClaimStatusEnum TotalClaimStatusId { get; set; } = ClaimStatusEnum.None;

        public string TotalClaimStatusValue { get; set; }

        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; } = ClaimLineItemStatusEnum.Unknown;

        public string ClaimLineItemStatusValue { get; set; }

        public ClaimStatusExceptionReasonCategoryEnum? ClaimStatusExceptionReasonCategoryId { get; set; } = ClaimStatusExceptionReasonCategoryEnum.Other;

        public string ExceptionReason { get; set; }

        public string ExceptionRemark { get; set; }


        [StringLength(72)]
        public string RemarkCode { get; set; }


        [StringLength(72)]
        public string ReasonCode { get; set; }

        public string ReasonDescription { get; set; }

        public string RemarkDescription { get; set; }

        public decimal? TotalClaimChargeAmount { get; set; }

        public decimal? TotalNonAllowedAmount { get; set; }

        public decimal? TotalAllowedAmount { get; set; }

        public decimal? TotalMemberResponsibilityAmount { get; set; }

        public decimal? DeductibleAmount { get; set; }

        public decimal? CopayAmount { get; set; }

        public decimal? CoinsuranceAmount { get; set; }

        public decimal? CobAmount { get; set; }

        public decimal? PenalityAmount { get; set; }

        public decimal? LineItemChargeAmount { get; set; }

        public decimal? LineItemPaidAmount { get; set; }

        public decimal? LineItemApprovedAmount { get; set; }


        [StringLength(25)]
        public string ClaimNumber { get; set; }


        [StringLength(64)]
        public string DiagnosisCode { get; set; }

        public DateTime? DateReceived { get; set; } = null;
        
        public string? DiagnosisDescription { get; set; }
        
        public DateTime? DateEntered { get; set; } = null;
        
        public string? ServiceLineDenialReason { get; set; }
        
        public DateTime? DatePaid { get; set; } = null;
        
        
        [StringLength(25)]
        public string CheckNumber { get; set; }

        public DateTime? CheckDate { get; set; } = null;

        public decimal? CheckPaidAmount { get; set; }
        
        public bool? AuthorizationFound { get; set; }
        
        public AuthorizationStatusEnum? AuthorizationStatusId { get; set; }        

        [StringLength(25)]
        public string AuthorizationNumber { get; set; }


        [StringLength(50)]
        public string EligibilityInsurance { get; set; } = string.Empty;


        [StringLength(25)]
        public string EligibilityPolicyNumber { get; set; } = string.Empty;

        public string EligibilityPhone { get; set; } = string.Empty;

        public string EligibilityUrl { get; set; } = string.Empty;

        public DateTime? CobLastVerified { get; set; }

        public string PrimaryPayer { get; set; }

        public string PrimaryPolicyNumber { get; set; }

        public string VerifiedMemberId { get; set; }

        public DateTime? EligibilityFromDate { get; set; }

        public string LastActiveEligibleDateRange { get; set; }

        [StringLength(16)]
        public string EligibilityStatus { get; set; } = string.Empty;

        public string ICN { get; set; }
        public string ReferringProviderName { get; set; }
        public string PlanType { get; set; }
        public string CurrentCoverage { get; set; }
        public string HippaStatus { get; set; }
        public string PaymentType { get; set; }

        public string BillingProviderNPI { get; set; }

        public DateTime? DateClaimFinalized { get; set; }

        public string COInsurerName { get; set; }

        public DateTime? CoInsurerEffectiveDate { get; set; }

        public DateTime? PartA_EligibilityFromDate { get; set; }

        public DateTime? PartA_EligibilityToDate { get; set; }

        public DateTime? PartA_DeductibleFromDate { get; set; }

        public DateTime? PartA_DeductibleToDate { get; set; }

        public decimal? PartA_RemainingDeductible { get; set; }

        public DateTime? PartB_EligibilityFromDate { get; set; }

        public DateTime? PartB_EligibilityToDate { get; set; }

        public DateTime? PartB_DeductibleFromDate { get; set; }

        public DateTime? PartB_DeductibleToDate { get; set; }

        public decimal? PartB_RemainingDeductible { get; set; }

        public DateTime? OtCapYearFrom { get; set; }

        public DateTime? OtCapYearTo { get; set; }

        public decimal? OtCapUsedAmount { get; set; }

        public DateTime? PtCapYearFrom { get; set; }

        public DateTime? PtCapYearTo { get; set; }

        public decimal? PtCapUsedAmount { get; set; }

        public int? InputDataListIndex { get; set; }

        public string InputDataFileName { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DbOperationEnum DbOperationId { get; set; }

        public decimal?  WriteoffAmount { get; set; }

		public string ErrorMessage { get; set; }

		public string CurlScript { get; set; }


		#region Navigational Property Access

		[ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ClaimStatusExceptionReasonCategoryId")]
        public virtual ClaimStatusExceptionReasonCategory ClaimStatusExceptionReasonCategory { get; set; }


        [ForeignKey("ClaimStatusTransactionId")]
        public virtual ClaimStatusTransaction ClaimStatusTransaction { get; set; }


        [ForeignKey("TotalClaimStatusId")]
        public virtual ClaimStatus TotalClaimStatus { get; set; }


        [ForeignKey("ClaimLineItemStatusId")]
        public virtual ClaimLineItemStatus ClaimLineItemStatus { get; set; }


        [ForeignKey("AuthorizationStatusId")]
        public virtual AuthorizationStatus AuthorizationStatus { get; set; }


        [ForeignKey("DbOperationId")]
        public virtual DbOperation DbOperation { get; set; }

        #endregion
    }
}
