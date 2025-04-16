using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum TransactionTypeEnum : int
    {
        [Description("Unknown")]
        Unknown = 0,

        [Description("Claim Status")]
        ClaimStatus = 1,

        [Description("Charge Entry")]
        ChargeEntry = 2,
    }
}
