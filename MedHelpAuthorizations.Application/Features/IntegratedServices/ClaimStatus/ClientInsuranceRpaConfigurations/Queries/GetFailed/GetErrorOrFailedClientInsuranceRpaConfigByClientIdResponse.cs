namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed
{
    public class GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse
    {
        public string ClientCode { get; set; }
        public string ClientInsuranceLookupName { get; set; }
        public string AuthTypeName { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ReportFailureToEmail { get; set; }
        public string FailureMessage { get; set; }
        public bool FailureReported { get; set; }
        public bool ExpiryWarningReported { get; set; }
        public string RpaGroupName { get; set; }
        public string Username { get; set; }
        public string TargetUrl { get; set; }
        public string Password { get; set; }
        public bool ShowPassword { get; set; } = false;
    }
}
