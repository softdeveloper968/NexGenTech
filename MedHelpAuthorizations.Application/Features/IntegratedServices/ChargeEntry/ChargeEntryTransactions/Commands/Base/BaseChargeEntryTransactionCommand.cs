using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Commands.Base
{
    public class BaseChargeEntryTransactionCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        
        public string PatientFullName { get; set; }

        public string PatientAccountNumber { get; set; }

        public int ChargeEntryBatchId { get; set; }

        public string LineItemControlNumber { get; set; }

        //Comma Delimited
        public string CommaDelimitedDxCodes { get; set; }

        public string CommaDelimitedProcedureCodes { get; set; }

        public string LocationName { get; set; }

        public DateTime DateOfServiceFrom { get; set; }

        public DateTime DateOfServiceTo { get; set; }

        public string PayerName { get; set; }

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

        public string Remarks { get; set; }

        public string ExceptionReason { get; set; }

        public bool SuccessfullyBilled { get; set; } = true;
    }
}
