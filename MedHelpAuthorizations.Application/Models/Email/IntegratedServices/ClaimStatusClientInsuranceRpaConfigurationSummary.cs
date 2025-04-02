
using System;

namespace MedHelpAuthorizations.Application.Models.Email.IntegratedServices
{
    public class ClientInsuranceRpaConfigurationSummary
    {
        public int ClientRpaConfigurationId { get; set; }
        public string ClientCode { get; set; }
        public string ClientInsuranceName { get; set; }
        public string AuthTypeName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TargetUrl { get; set; }
        public string ReportFailureToEmail { get; set; }
        public int EffectedBatchCount { get; set; }
        public DateTime? FailureRecordedOn { get; set; }
        public string FailureMessage { get; set; }
        public string ExpiryWarningReported { get; set; }
    }
}
