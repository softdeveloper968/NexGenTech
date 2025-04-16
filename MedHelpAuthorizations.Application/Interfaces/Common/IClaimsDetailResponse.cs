using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Interfaces.Common
{
    public interface IClaimsDetailResponse
    {
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PolicyNumber { get; set; }
        public int? ClientInsuranceId { get; set; }
        public string InsuranceName { get; set; }

        [StringLength(16)]
        public string OfficeClaimNumber { get; set; }

        [StringLength(16)]
        public string InsuranceClaimNumber { get; set; }
        //public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
        public DateTime ClaimBilledOn { get; set; }
        public DateTime DateOfService { get; set; }
        public string ProcedureCode { get; set; }
        //public string Modifiers { get; set; }
        // public int Quantity { get; set; }
        public string ProviderName { get; set; }
        public decimal? BilledAmount { get; set; }
        public int? ClientLocationId { get; set; }
        public string ClientLocationName { get; set; }
        public string ClientLocationNpi { get; set; }
    }
}
