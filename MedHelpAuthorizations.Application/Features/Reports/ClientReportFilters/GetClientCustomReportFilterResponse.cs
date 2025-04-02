using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class GetClientCustomReportFilterResponse : GetClientReportFilterResponse
    {
        public GetClientCustomReportFilterResponse() { }

        public List<EmployeeClientReportFilterDTO> EmployeeClientReportFilter { get; set; } = new();
        public bool IsSharedReport { get; set; } = false;
        public string CreatedByUserName { get; set; }
        public List<string> CustomReportSharedWithUserIds { get; set; } = new();
        public List<int> CustomReportSharedWithUserEmployeeIds { get; set; } = new();
    }
    public class ClientCustomReportFilterDetails
    {
        public ClientCustomReportFilterDetails() { }
        public List<GetClientCustomReportFilterResponse> CustomReportFilterResponses { get; set; } = new();
        public Dictionary<string, string> UserNamesDict { get; set; } = new();
    }
}
