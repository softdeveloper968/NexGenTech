using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Queries.Base;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Queries.GetAll
{
    public class GetAllInputDocumentsResponse : GetInputDocumentResponse
    {
		public int ImportDocumentErrorMessagesCount { get; set; } = 0;
		public int ImportDocumentUnMatchedLocationAndProviderMessagesCount { get; set; } = 0;
		public int ImportDocumentRepeatMessagesCount { get; set; } = 0;
        public decimal FileUploadPercentage { get; set; }
    }
}
