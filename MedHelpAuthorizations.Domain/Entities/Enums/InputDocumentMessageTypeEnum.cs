using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
	public enum InputDocumentMessageTypeEnum : int
	{
		[Description("Errored")]
		Errored = 0,

		[Description("Unmatched Provider")]
		UnmatchedProvider = 1,

		[Description("Unmatched Location")]
		UnmatchedLocation = 2,

		[Description("File Duplicates")]
        FileDuplicates = 3,

		[Description("UnSupplantableDuplicates")]
        UnSupplantableDuplicates = 4,
    }
}
