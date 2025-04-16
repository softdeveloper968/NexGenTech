using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum RelativeDateRangeEnum : int
    {
        [Description("1st of month to DateTime.UtcNow")]
        FirstOfMonthToUtcNow = 1,

        [Description("1st of year to DateTime.UtcNow")]
        FirstOfYearToUtcNow = 2,

        [Description("DateTime.Min to DateTime.UtcNow")]
        DateMinToUtcNow = 3,
    }
}
