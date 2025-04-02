using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientRpaCredentialConfigurations.Queries.GetAll
{
    public class GetAllClientRpaCredentialConfigurationsResponse
    {
        public int Id { get; set; }
        public int? RpaInsuranceGroupId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AlternateUsername { get; set; }
        public string AlternatePassword { get; set; }
        public string FailureMessage { get; set; }
        public string ReportFailureToEmail { get; set; }
        public bool ExpiryWarningReported { get; set; } = false;
        public bool IsCredentialInUse { get; set; }
        public bool UseOffHoursOnly { get; set; } = false;
        public string OtpForwardFromEmail { get; set; }
        public string RpaGroupName { get; set; }
        public string DefaultTargetUrl { get; set; }
        public bool FailureReported { get; set; }
    }
}
