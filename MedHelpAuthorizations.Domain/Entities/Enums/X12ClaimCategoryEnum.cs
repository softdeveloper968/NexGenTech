
using System.ComponentModel;

namespace MedHelpAuthorizations.Domain.Entities.Enums
{
	public enum X12ClaimCategoryEnum
	{
		[Description("Acknowledgment")]
		Acknowledgment = 1,

		[Description("Data Reporting Acknowledgment")]
		DataReportingAcknowledgment = 2,

		[Description("Pending")]
		Pending = 3,

		[Description("Finalized")]
		Finalized = 4,

		[Description("Requests For Additional Info")]
		RequestsForAdditionalInfo = 5,

		[Description("General")]
		General = 6,

		[Description("Error")]
		Error = 7,

		[Description("Searches")]
		Searches = 8,
	}
}
