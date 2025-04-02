using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Queries.Base
{
	public class GetInputDocumentResponse
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; } = string.Empty;
		public int? ClientId { get; set; }
		public string URL { get; set; }
		public InputDocumentTypeEnum? InputDocumentTypeId { get; set; }
		public DateTime? DocumentDate { get; set; }
		public DateTime CreatedOn { get; set; }
		public string CreatedBy { get; set; }
		public bool IsPublic { get; set; }
		public bool IsDeleted { get; set; } = false;
		public ImportStatusEnum ImportStatus { get; set; }
		public List<Domain.Entities.ImportDocumentMessage> ImportDocumentMessages { get; set; }
		public int? ClaimStatusBatchId { get; set; }
		public string ClaimStatusBatchNumber { get; set; }
		public string ClaimStatusBatchStatus { get; set; }
		public HashSet<ClaimStatusBatch> ClaimStatusBatches { get; set; } = new();
		public int AttemptedImportCount { get; set; }
		public int ActualImportCount { get; set; }
		public string ErrorMessage { get; set; }//EN-652

		//public ClaimStatusBatch ClaimStatusBatch { get; set; }
	}
}
