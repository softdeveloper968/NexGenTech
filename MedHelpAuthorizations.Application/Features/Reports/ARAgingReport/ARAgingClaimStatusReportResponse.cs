using MedHelpAuthorizations.Domain.Entities.Enums;
using System;

namespace MedHelpAuthorizations.Application.Features.Reports.ARAgingReport
{
    public class ARAgingSummaryReportResponse : ARAgingInsurance
    {
        //public int Quantity { get; set; } = default(int);
        public decimal ChargedSum { get; set; } = 0.0m;
        public string LocationName { get; set; } = string.Empty;
        public string ProviderName { get; set; } = string.Empty;
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
    }
    public class ARAgingExportDetailsResponse
    {
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string PayerName { get; set; }
        public string PolicyNumber { get; set; }
        public string ServiceType { get; set; }
        public string DateOfServiceFrom { get; set; }
        public string ProcedureCode { get; set; }
        public decimal BilledAmount { get; set; }
        public string ClaimBilledOn { get; set; }
        public string ClaimLineItemStatus { get; set; }
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
        public string ExceptionReasonCategory { get; set; }
        public string ExceptionReason { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string LocationNpi { get; set; } = string.Empty;


    }
}
