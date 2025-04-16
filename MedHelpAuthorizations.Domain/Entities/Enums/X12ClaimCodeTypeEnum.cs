using CsvHelper.Configuration.Attributes;
using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
	public enum X12ClaimCodeTypeEnum
    {
        [Name("RARC")]
        [Description("Remittance Advice Remark Code")]
        RARC = 1,

        [Name("CARC")]
        [Description("Claim Adjustment Reason Code")]
        CARC = 2,

        [Name("REMARK")]
        [Description("Remark Code")]
        REMARK = 3,

        [Name("ICES")]
        [Description("Optum Claims Editing System")]
        ICES = 4,
    }
}
