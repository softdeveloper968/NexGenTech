using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ApplicationReportEnum : int
    {
        /// <summary>
        ///  Aceess to Authorizations Features
        /// </summary>
        [Description("Daily Claim Status Report")]
        DailyClaimStatusReport = 1
    }
}
