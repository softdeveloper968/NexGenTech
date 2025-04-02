namespace MedHelpAuthorizations.Application.Requests.IntegratedServices.InputDocuments
{
    public class GetAllPagedInputDocumentsRequest : PagedRequest
    {
        public string SearchString { get; set; }
    }
}