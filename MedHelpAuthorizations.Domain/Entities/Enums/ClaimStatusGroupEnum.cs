using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ClaimStatusGroupEnum : int
    {
        [Description("Export All")]
        ExportAll,

        [Description("Other")]
        Other,

        [Description("Paid/Approved")]
        PaidApproved,

        [Description("Not-Adjudicated")]
        NotAdjudicated,

        [Description("Denied")]
        Denied,

        [Description("Zero Pay")]
        ZeroPay,

        [Description("Reviewed")]
        Reviewed,

        [Description("InProcess")]
        InProcess
    }
}
