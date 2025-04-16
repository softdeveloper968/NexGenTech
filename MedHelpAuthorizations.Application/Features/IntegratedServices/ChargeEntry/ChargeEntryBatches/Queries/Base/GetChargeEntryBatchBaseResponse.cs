using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Base
{
    public class GetChargeEntryBatchBaseResponse
    {
        public int Id { get; set; }

        public string BatchNumber { get; set; }

        public int ChargeEntryRpaConfigurationId { get; set; }

        public DateTime DateOfServiceFrom { get; set; }

        public DateTime DateOfServiceTo { get; set; }

        public string ProcessStartedByRpaCode { get; set; }

        public string ClientCode { get; set; }

        public int ClientId { get; set; }

        public string RpaTypeDescription { get; set; }

        public string AuthTypeName { get; set; }

        public string TransactionTypeName { get; set; }

        public DateTime? ProcessStartDateTimeUtc { get; set; }

        public DateTime? CompletedDateTimeUtc { get; set; }

        public DateTime? AbortedOnUtc { get; set; }

        public string AbortedReason { get; set; }

        public DateTime? ReviewedOnUtc { get; set; }

        public string ReviewedBy { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
