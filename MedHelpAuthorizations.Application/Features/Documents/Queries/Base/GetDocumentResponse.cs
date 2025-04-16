namespace MedHelpAuthorizations.Application.Features.Documents.Queries.Base
{
    public class GetDocumentResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public bool IsPublic { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string URL { get; set; }
    }
}
