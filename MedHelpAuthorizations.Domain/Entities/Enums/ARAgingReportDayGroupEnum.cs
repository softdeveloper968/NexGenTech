using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ARAgingReportDayGroupEnum : int
    {
        [Description("0-30")]
        AgeGroup_0_30 = 0,

        [Description("31-60")]
        AgeGroup_31_60 = 1,

        [Description("61-90")]
        AgeGroup_61_90 = 2,

        [Description("91-120")]
        AgeGroup_91_120 = 3,

        [Description("121-150")]
        AgeGroup_121_150 = 4,

        [Description("151-180")]
        AgeGroup_151_180 = 5,

        [Description("181+")]
        AgeGroup_181_plus = 6
    }
    public enum AgingReportExportOption : int
    {
        [Description("Export Details")]
        Details = 0,
        [Description("Export Summary")]
        Summary = 1
    }
}
