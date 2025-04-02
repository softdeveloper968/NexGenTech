using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ReportsEnum : int
    {
        [Description("A/R Aging Summary")]
        AR_Aging_Summary = 1,

        [Description("A/R Aging Summary with Payment Info")]
        AR_Aging_Summary_With_Payment_info = 2,

        [Description("Activity Summary")]
        Activity_Summary = 3,

        [Description("Activity Summary By Charge Status")]
        Activity_Summary_By_Charge_Status = 4,

        [Description("Daily Claim Report")]
        Daily_Claim_Report = 5,

        [Description("Custom Report")]
        Custom_Reports = 6,

    }
}
