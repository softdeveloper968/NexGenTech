using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetAll
{
	public class ImportDocumentMessageResponse
	{
		public int Id { get; set; }
		public int ClaimStatusBatchId { get; set; }
		public int InputDocumentId { get; set; }
		public InputDocumentMessageTypeEnum MessageType { get; set; }
		public string Message { get; set; }
		public int? ClaimStatusBatchClaimId { get; set; }
	}
}
