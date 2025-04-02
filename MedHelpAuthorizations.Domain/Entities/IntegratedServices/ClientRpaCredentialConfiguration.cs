using MedHelpAuthorizations.Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedHelpAuthorizations.Domain.Entities.IntegratedServices
{
    public class ClientRpaCredentialConfiguration : AuditableEntity<int>
    {
        public ClientRpaCredentialConfiguration() { }
        public int? RpaInsuranceGroupId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FailureMessage { get; set; }
        public string ReportFailureToEmail { get; set; }
        public bool ExpiryWarningReported { get; set; } = false;
        public bool IsCredentialInUse { get; set; }
        public bool UseOffHoursOnly { get; set; } = false;
        public string OtpForwardFromEmail { get; set; }
        public bool FailureReported { get; set; } = false;

		#region Navigational Property Access

		[ForeignKey("RpaInsuranceGroupId")]
        public virtual RpaInsuranceGroup RpaInsuranceGroup { get; set; }
        #endregion
    }
}
