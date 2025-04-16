using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Models.IntegratedServices
{
    public class ClaimsDetailResponse : IClaimsDetailResponse
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
        public DateTime ClaimBilledOn { get; set; }
        public DateTime DateOfService { get; set; }

        [StringLength(16)]
        public string ProcedureCode { get; set; }
        public string ProviderName { get; set; }
        public decimal? BilledAmount { get; set; }
        public int? ClientLocationId { get; set; }
        public string ClientLocationName { get; set; }
        public string ClientLocationNpi { get; set; }
        public int? ClientInsuranceId { get; set; }
    }
}
