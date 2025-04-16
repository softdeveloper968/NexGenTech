using MedHelpAuthorizations.Shared.Requests;

namespace MedHelpAuthorizations.Application.Requests.Administration
{
    public class GetAllPagedFeeScheduleEntryRequest : PagedRequest
    {
        public string SearchString { get; set; } = string.Empty;
        public int ClientFeeScheduleId { get; set; }
        public string SortLabel {  get; set; } = string.Empty;
        public string SortDirection {  get; set; } = string.Empty;
    }
}
