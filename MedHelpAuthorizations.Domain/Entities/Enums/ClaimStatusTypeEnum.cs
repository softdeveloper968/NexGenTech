using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
    public enum ClaimStatusTypeEnum : int 
    {
        [Description("PaidClaimStatusType")]
        PaidClaimStatusType = 1,

        [Description("DeniedClaimStatusType")]
        DeniedClaimStatusType = 2,

        [Description("OpenClaimStatusType")]
        OpenClaimStatusType = 3,

		[Description("OtherAdjudicatedClaimStatusType")]
		OtherAdjudicatedClaimStatusType = 4,

		[Description("OtherOpenClaimStatusType")]
		OtherOpenClaimStatusType = 5,
	}
}
