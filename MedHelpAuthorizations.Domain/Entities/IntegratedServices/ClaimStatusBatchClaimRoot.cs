using MedHelpAuthorizations.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ClaimStatusBatchClaimRoot : AuditableEntity<int>, ISoftDelete
    {
        public ClaimStatusBatchClaimRoot()
        {
            ClaimStatusBatchClaims = new HashSet<ClaimStatusBatchClaim>();
        }

        public ClaimStatusBatchClaimRoot(int clientId, string patientFirstName, string patientLastName, string policyNumber, DateTime dateOfBirth, DateTime dateOfServiceFrom, DateTime dateOfServiceTo, DateTime? claimBilledOn, string procedureCode, int clientProviderId, int clientLocationId)//, int clientLocationId)
        {
            //Required
            ClientId = clientId;
            PatientFirstName = patientFirstName;
            PatientLastName = patientLastName;
            PolicyNumber = policyNumber;
            DateOfBirth = dateOfBirth;
            DateOfServiceFrom = dateOfServiceFrom;
            DateOfServiceTo = dateOfServiceTo;
            ProcedureCode = procedureCode;
        }

        public int? ClientId { get; set; }

        public int? ClientProviderId { get; set; }

        public int? ClientLocationId { get; set; }


        [StringLength(64)]
        public string? PatientLastCommaFirstName => $"{PatientLastName}, {PatientFirstName}";

        [Required]
        [StringLength(24)]
        public string PatientLastName { get; set; }

        [Required]
        [StringLength(24)]
        public string PatientFirstName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string DateOfBirthString { get; set; }


        [Required]
        [StringLength(32)]
        public string PolicyNumber { get; set; }

        public DateTime? DateOfServiceFrom { get; set; }

        public string DateOfServiceFromString { get; set; }

        public DateTime? DateOfServiceTo { get; set; }

        public string DateOfServiceToString { get; set; }

        public DateTime? ClaimBilledOn { get; set; }

        public string ClaimBilledOnString { get; set; }

        [Required]
        [StringLength(16)]
        public string ProcedureCode { get; set; }


        [StringLength(30)]
        public string Modifiers { get; set; }

        public decimal? BilledAmount { get; set; }

        public decimal? BilledAmountString { get; set; }

        public int Quantity { get; set; } = 1;

        public bool IsDeleted { get; set; } = false;


        [StringLength(10)]
        public string RenderingNpi { get; set; }


        [StringLength(10)]
        public string GroupNpi { get; set; }


        [StringLength(16)]
        public string ClaimNumber { get; set; }


        [StringLength(34)]
        public string EntryMd5Hash { get; set; }


        #region Navigational Property Access

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        public virtual ICollection<ClaimStatusBatchClaim> ClaimStatusBatchClaims { get; set; }

        [ForeignKey("ClientProviderId")]
        public virtual ClientProvider ClientProvider { get; set; }


        [ForeignKey("ClientLocationId")]
        public virtual ClientLocation ClientLocation { get; set; }
        #endregion
    }
}
