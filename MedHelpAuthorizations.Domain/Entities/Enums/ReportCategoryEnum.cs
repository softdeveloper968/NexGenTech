using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ReportCategoryEnum : int
    {
        [Description("AR Management Report")]
        AR_Management_Report = 1,

        [Description("AR Reports")]
        AR_Reports = 2,

        [Description("Claim Reports")]
        Claim_Reports = 3,

        [Description("Daily/Monthly Reports")]
        Daily_Monthly_Reports = 4,

        [Description("Facility Reports")]
        Facility_Reports = 5,

        [Description("Facility Requested Reports")]
        Facility_Requested_Reports = 6,

        [Description("Management Reports")]
        Management_Reports = 7,

        [Description("Misc Reports")]
        Misc_Reports = 8,

        [Description("Patients Reports")]
        Patients_Reports = 9,

        [Description("Custom Reports")]
        Custom_Reports = 10,

    }
}
