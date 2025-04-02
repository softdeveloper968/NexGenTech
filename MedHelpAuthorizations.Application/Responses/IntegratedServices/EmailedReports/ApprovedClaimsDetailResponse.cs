using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports
{
    public class ApprovedClaimsDetailResponse : IClaimsDetailResponse
    {
        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PolicyNumber { get; set; }
        public string InsuranceName { get; set; }

        [StringLength(16)]
        public string OfficeClaimNumber { get; set; }

        [StringLength(16)]
        public string InsuranceClaimNumber { get; set; }
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }
        public DateTime? ApprovedSinceDate { get; set; }
        public DateTime ClaimBilledOn { get; set; }
        public DateTime DateOfService { get; set; }

        [StringLength(16)]
        public string ProcedureCode { get; set; }

        [StringLength(30)]
        public string Modifiers { get; set; }
        public int Quantity { get; set; } = 1;
        //public string LocationName { get; set; }
        public string ProviderName { get; set; }
        public decimal? BilledAmount { get; set; }
        public int? ClientLocationId { get; set; }
        public string ClientLocationName { get; set; }
        public string ClientLocationNpi { get; set; }
        public int? ClientInsuranceId { get; set; }
    }
}
