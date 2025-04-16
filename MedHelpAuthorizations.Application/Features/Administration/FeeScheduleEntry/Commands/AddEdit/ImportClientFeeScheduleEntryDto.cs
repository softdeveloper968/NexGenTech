using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Features.Administration.FeeScheduleEntry.Commands.AddEdit
{
	public class ImportClientFeeScheduleEntryDto
	{
		public int ClientFeeScheduleId { get; set; }
		public int ClientId { get; set; }
		public UploadRequest UploadRequest { get; set; }
		public string URL { get; set; }

	}
}
