namespace MedHelpAuthorizations.Application.Features.Documents.Queries.Base
{
    public class InputDocumentFileStatusResponse
    {
        public int InputDocumentId { get; set; }
        public int AttemptCount { get; set; }
        public int ActualCount { get; set; }
        public int FileUploadPercentage { get; set; }
    }
}
