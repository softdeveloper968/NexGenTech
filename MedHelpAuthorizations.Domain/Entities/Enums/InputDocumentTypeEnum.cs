using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum InputDocumentTypeEnum : int
    {
        [Description("Claim Status Input")]
        ClaimStatusInput = 1,

        [Description("Claim Status")]
        ClaimStatusWriteOff = 2,
    }
}
