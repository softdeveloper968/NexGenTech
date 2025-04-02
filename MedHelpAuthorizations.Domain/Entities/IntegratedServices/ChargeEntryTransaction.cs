using MedHelpAuthorizations.Domain.Common.Contracts;
using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ChargeEntryTransaction : AuditableEntity<int>//, ITenant
    {
        public ChargeEntryTransaction()
        {
            ChargeEntryTransactionHistories = new HashSet<ChargeEntryTransactionHistory>();
        }

        [Required]
        public string PatientFullName { get; set; }

        [Required]
        public string PatientAccountNumber { get; set; }

        [Required]
        public int ChargeEntryBatchId { get; set; }

        //Comma Delimited
        public string CommaDelimitedDxCodes { get; set; }

        public string CommaDelimitedProcedureCodes { get; set; }

        public string LocationName { get; set; }

        public DateTime DateOfServiceFrom { get; set; }

        public DateTime DateOfServiceTo { get; set; }

        public DateTime ChargeEntryTransactionBeginDateTimeUtc { get; set; }

        public DateTime ChargeEntryTransactionEndDateTimeUtc { get; set; }

        public string PayerName { get; set; }

        public string OriginalPrimaryInsurance { get; set; }

        public string OriginalSecondaryInsurance { get; set; }

        public string CorrectedPrimaryInsurance { get; set; }

        public string CorrectedSecondaryInsurance { get; set; }

        public string OriginalNpi { get; set; }

        public string OriginalTaxId { get; set; }

        public string CorrectedNpi { get; set; }

        public string CorrectedTaxId { get; set; }

        public string LineItemControlNumber { get; set; }

        public bool SuccessfullyBilled { get; set; } = true;

        public string ExceptionReason { get; set; }

        public string Remarks { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? ClientId { get; set; }

       //public string TenantId { get; set; }


        [StringLength(72)]
        public string UiPathUniqueReference { get; set; }


        #region Navigational Property Init

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public virtual ICollection<ChargeEntryTransactionHistory> ChargeEntryTransactionHistories { get; set; }

        #endregion

    }
}
