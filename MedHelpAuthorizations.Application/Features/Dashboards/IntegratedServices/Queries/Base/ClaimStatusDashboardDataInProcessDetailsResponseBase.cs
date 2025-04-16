using System;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base
{
    public class ClaimStatusDashboardInProcessDetailsResponseBase : IClaimStatusDashboardInProcessDetailsResponseBase
    {
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string DateOfBirth { get; set; }
        public string PayerName { get; set; }
        public string ServiceType { get; set; }
        public string PolicyNumber { get; set; }
        public string OfficeClaimNumber { get; set; }
        public string ClaimBilledOn { get; set; }
        public string DateOfServiceFrom { get; set; }
        public string DateOfServiceTo { get; set; }
        public string ProcedureCode { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal BilledAmount { get; set; }
		public int ClaimStatusBatchId { get; set; }
		public string BatchNumber { get; set; }
        public string AitClaimReceivedDate { get; set; }
        public string AitClaimReceivedTime { get; set; }
        public string ClientProviderName {  get; set; }
        public int? PatientId { get; set; }
        public string ClientLocationName { get; set; }
        public string ClientLocationNpi { get; set; }
        public string PaymentType { get; set; } //AA-324
    }
}
