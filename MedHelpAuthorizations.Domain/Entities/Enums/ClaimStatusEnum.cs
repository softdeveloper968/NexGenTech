using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ClaimStatusEnum : int
    {
       
        [Description("None")]
        None = 0,

        [Description("Completed")]
        Completed = 1,

        [Description("Rejected")]
        Rejected = 2,

        [Description("Voided")]
        Voided = 3,

        [Description("In-Process")]
        InProcess = 4,

        [Description("Received")]
        Received = 5,

        [Description("Not-Adjudicated")]
        NotAdjudicated = 6,

        [Description("Acknowledged")]
        Acknowledged = 7
    }
}
