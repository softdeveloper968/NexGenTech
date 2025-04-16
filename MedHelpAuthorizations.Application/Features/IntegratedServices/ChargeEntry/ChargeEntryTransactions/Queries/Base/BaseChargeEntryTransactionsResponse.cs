using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.Base
{
    public class BaseChargeEntryTransactionsResponse
    {
        public int Id { get; set; }
        public int ChargeEntryBatchId { get; set; }
        public string PayerName { get; set; }
        public string PatientFullName { get; set; }
        public string PatientAccountNumber { get; set; }
        public DateTime? DateOfServiceFrom { get; set; }
        public DateTime? DateOfServiceTo { get; set; }
        public string LocationName { get; set; }
        public string LineItemControlNumber { get; set; }
        public string CommaDelimitedDxCodes { get; set; }
        public string CommaDelimitedProcedureCodes { get; set; }
        public DateTime ChargeEntryTransactionBeginDateTimeUtc { get; set; }
        public DateTime ChargeEntryTransactionEndDateTimeUtc { get; set; }
        public string OriginalPrimaryInsurance { get; set; }
        public string OriginalSecondaryInsurance { get; set; }
        public string CorrectedPrimaryInsurance { get; set; }
        public string CorrectedSecondaryInsurance { get; set; }
        public string OriginalNpi { get; set; }
        public string OriginalTaxId { get; set; }
        public string OriginalDiagnosis { get; set; }
        public string OriginalProcedure { get; set; }
        public string OriginalChargeAmount { get; set; }
        public string OriginalLocation { get; set; }
        public bool SuccessfullyBilled { get; set; } = true;
        public string ExceptionReason { get; set; }
        public string Remarks { get; set; }
        public string UiPathUniqueReference { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
