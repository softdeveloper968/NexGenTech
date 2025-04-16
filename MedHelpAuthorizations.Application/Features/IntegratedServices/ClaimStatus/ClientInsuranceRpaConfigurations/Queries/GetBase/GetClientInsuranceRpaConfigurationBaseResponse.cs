using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetBase
{
	public class GetClientInsuranceRpaConfigurationBaseResponse
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ClientInsuranceId { get; set; }
        public int RpaInsuranceId { get; set; }
        public int? RpaInsuranceGroupId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }
        public int? AuthTypeId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
		public string AlternateUsername { get; set; }
		public string AlternatePassword { get; set; }
		public string TargetUrl { get; set; }
        public string ExternalId { get; set; }
        public bool FailureReported { get; set; }
        public string FailureMessage { get; set; }
        public bool IsCredentialInUse { get; set; }
        public bool IsAlternateCredentialInUse { get; set; }
        public bool UseOffHoursOnly { get; set; }
        public string OtpForwardFromEmail { get; set; }  
        public bool IsDeleted { get; set; }
        public int? ClientRpaCredentialConfigId { get; set; }
		public int? AlternateClientRpaCredentialConfigId { get; set; }
		public string ClientInsuranceName { get; set; }
        public bool ExpiryWarningReported { get; set; }
        public int? ClientLocationId { get; set; }
        public string ClientLocationName { get; set; }
	}
}
