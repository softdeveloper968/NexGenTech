using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace MedHelpAuthorizations.Application.Responses.IntegratedServices.EmailedReports
{
    /// <summary>
    /// Represents a response model for detailed information about unknown status claims.
    /// </summary>
    public class UnknownClaimsDetailResponse : IClaimsDetailResponse
    {
        /// <summary>
        /// Gets or sets the first name of the patient.
        /// </summary>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the patient.
        /// </summary>
        public string PatientLastName { get; set; }

        /// <summary>
        /// Gets or sets the date of birth of the patient.
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets the policy number.
        /// </summary>
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Gets or sets the name of the insurance.
        /// </summary>
        public string InsuranceName { get; set; }

        /// <summary>
        /// Gets or sets the office claim number with a maximum length of 16 characters.
        /// </summary>
        [StringLength(16)]
        public string OfficeClaimNumber { get; set; }

        /// <summary>
        /// Gets or sets the insurance claim number with a maximum length of 16 characters.
        /// </summary>
        [StringLength(16)]
        public string InsuranceClaimNumber { get; set; }

        /// <summary>
        /// Gets or sets the status of the claim line item.
        /// </summary>
        public ClaimLineItemStatusEnum? ClaimLineItemStatusId { get; set; }

        /// <summary>
        /// Gets or sets the date on which the claim was billed.
        /// </summary>
        public DateTime ClaimBilledOn { get; set; }

        /// <summary>
        /// Gets or sets the date of service for the claim.
        /// </summary>
        public DateTime DateOfService { get; set; }

        /// <summary>
        /// Gets or sets the procedure code with a maximum length of 16 characters.
        /// </summary>
        [StringLength(16)]
        public string ProcedureCode { get; set; }

        /// <summary>
        /// Gets or sets the modifiers with a maximum length of 30 characters.
        /// </summary>
        [StringLength(30)]
        public string Modifiers { get; set; }

        /// <summary>
        /// Gets or sets the quantity, with a default value of 1.
        /// </summary>
        public int Quantity { get; set; } = 1;

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the billed amount for the claim.
        /// </summary>
        public decimal? BilledAmount { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the client's location.
        /// </summary>
        public int? ClientLocationId { get; set; }

        /// <summary>
        /// Gets or sets the name of the client's location.
        /// </summary>
        public string ClientLocationName { get; set; }

        /// <summary>
        /// Gets or sets the NPI (National Provider Identifier) for the client's location.
        /// </summary>
        public string ClientLocationNpi { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the client's insurance.
        /// </summary>
        public int? ClientInsuranceId { get; set; }

        /// <summary>
        /// Gets or sets the date of service as a string.
        /// </summary>
        public string DateOfServiceString { get; set; }

        /// <summary>
        /// Gets or sets the name of the location.
        /// </summary>
        public string LocationName { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider with the last name followed by the first name.
        /// </summary>
        public string ProviderNameLastCommaFirst { get; set; }

        /// <summary>
        /// Gets or sets the batch number.
        /// </summary>
        public string BatchNumber { get; set; }

        /// <summary>
        /// Gets or sets the service type.
        /// </summary>
        public string ServiceType { get; set; }
    }

}
