using System;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Base
{
    public class GetClaimStatusBatchClaimBaseResponse
    {
        public int Id { get; set; }

        public string ClaimNumber { get; set; }

        public string PayerClaimNumber { get; set; }

        public int ClaimStatusBatchId { get; set; }

        public string PatientLastCommaFirstName => $"{PatientLastName}, {PatientFirstName}";

        public string PatientLastName { get; set; }

        public string PatientFirstName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthString
        {
            get
            {
                return this.DateOfBirth != null ? DateOfBirth.Value.ToString("MM/dd/yyyy") : string.Empty;
            }
        }

        public string PolicyNumber { get; set; }

		public DateTime? PolicyNumberUpdatedOn { get; set; }

		public DateTime? DateOfServiceFrom { get; set; }

        public string DateOfServiceFromString
        {
            get
            {

                return this.DateOfServiceFrom != null ? DateOfServiceFrom.Value.ToString("MM/dd/yyyy") : string.Empty;
            }
        }

        public DateTime? DateOfServiceTo { get; set; }

        public string DateOfServiceToString
        {
            get
            {

                return this.DateOfServiceTo != null ? DateOfServiceTo.Value.ToString("MM/dd/yyyy") : string.Empty;
            }
        }

        public string ProcedureCode { get; set; }

        public string Modifiers { get; set; }

        public DateTime?  ClaimBilledOn { get; set; }

        public string ClaimBilledOnString
        {
            get
            {

                return this.ClaimBilledOn != null ? ClaimBilledOn.Value.ToString("MM/dd/yyyy") : string.Empty;
            }
        }

        public decimal? BilledAmount { get; set; }

        public string BilledAmountString
        {
            get
            {

                return this.BilledAmount != null ? BilledAmount.Value.ToString() : string.Empty;
            }
        }

        public int Quantity { get; set; } = 1;

        int? InputDataListIndex { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string TaxId { get; set; }

        public string RenderingNpi { get; set; }

        public string GroupNpi { get; set; }

        public string ClientLocationNpi { get; set; }

        public int? ClaimStatusTransactionId { get; set; }

        public DateTime? LastStatusChangedOn { get; set; }
    }
}
