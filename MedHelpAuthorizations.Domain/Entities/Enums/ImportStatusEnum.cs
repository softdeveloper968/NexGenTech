using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ImportStatusEnum : int
    {
        [Description("N/A")]
		NotApplicable = 0,

        [Description("Pending")]
        Pending = 1,

        [Description("Completed")]
        Completed = 2,

        [Description("Error")]
        Error = 3
    }
}
