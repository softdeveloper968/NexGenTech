using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ChargeEntryTransactionHistory : AuditableEntity<int>//, ITenant
    {
        public ChargeEntryTransactionHistory()
        {

        }

        public ChargeEntryTransactionHistory(int claimStatusTransactionId, int chargeEntryBatchId, string patientFullName, DateTime claimStatusTransactionBeginDateTimeUtc, DateTime claimStatusTransactionEndDateTimeUtc, DbOperationEnum dbOperationId)
        {
            ChargeEntryTransactionId = claimStatusTransactionId;
            ChargeEntryTransactionBeginDateTimeUtc = claimStatusTransactionBeginDateTimeUtc;
            ChargeEntryTransactionEndDateTimeUtc = claimStatusTransactionEndDateTimeUtc;
            DbOperationId =dbOperationId;
            PatientFullName = patientFullName;

            #region Navigational Property Init

            #endregion
        }

        [Required]
        public int ChargeEntryTransactionId { get; set; }

        [Required] public string PatientFullName { get; set; }

        public string PatientAccountNumber { get; set; }

        [Required]
        public int ChargeEntryBatchId { get; set; }

        public DateTime ChargeEntryTransactionBeginDateTimeUtc { get; set; }

        public DateTime ChargeEntryTransactionEndDateTimeUtc { get; set; }

        public string OriginalPrimaryInsurance { get; set; }

        public string OriginalSecondaryInsurance { get; set; }

        public string CorrectedPrimaryInsurance { get; set; }

        public string CorrectedSecondaryInsurance { get; set; }

        public string OriginalNpi { get; set; }

        public string OriginalTaxId { get; set; }

        public string CorrectedNpi { get; set; }

        public string CorrectedTaxId { get; set; }

        public string LineItemControlNumber { get; set; }

        [StringLength(72)]
        public string UiPathUniqueReference { get; set; }

        public bool Successful { get; set; } = true;

        public string ExceptionReason { get; set; }

        public string Message { get; set; }

        public bool IsDeleted { get; set; } = false;

        public DbOperationEnum DbOperationId { get; set; }
        //public string TenantId { get; set; }

         public int? ClientId { get; set; }


        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        //public virtual ICollection<ChargeEntryTransactionHistory> ChargeEntryTransactionHistories { get; set; }

        //[ForeignKey("ChargeEntryBatchClaimId")]
        //public virtual ChargeEntryBatchClaim ChargeEntryBatchClaim { get; set; }


        //[ForeignKey("TotalChargeEntryId")]
        //public virtual ChargeEntry TotalChargeEntry { get; set; }


        //[ForeignKey("ClaimLineItemStatusId")]
        //public virtual ClaimLineItemStatus ClaimLineItemStatus { get; set; }


        //[ForeignKey("AuthorizationStatusId")]
        //public virtual AuthorizationStatus AuthorizationStatus { get; set; }
    }
}
