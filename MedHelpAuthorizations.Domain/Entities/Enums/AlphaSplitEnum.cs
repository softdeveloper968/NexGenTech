using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum AlphaSplitEnum : int
    {
        //[Description("")]
        //None = 0,

        [Description("Custom Range")]
        CustomRange = 1,

        [Description("A-G")]
        AtoG,

        [Description("H-L")]
        HtoL,

        [Description("M-R")]
        MtoR,

        [Description("S-Z")]
        StoZ,
    }
}
