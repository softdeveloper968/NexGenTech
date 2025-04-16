using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;

namespace MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters
{
    public class EmployeeClientReportFilterDTO
    {
        public EmployeeClientReportFilterDTO() { }
        public int Id { get; set; }

        /// <summary>
        /// Client User Report Filter report Id [Original ReportId]
        /// </summary>
        public int ClientUserReportFilterId { get; set; }
        /// <summary>
        /// Shared Employee Id
        /// </summary>
        public int EmployeeClientId { get; set; }
        /// <summary>
        /// When any report is “saved as”.. it will record the Id of the base report (BaseReportId).
        /// user will allow to CRUD.
        /// </summary>
        public int? BaseClientUserReportFilterId { get; set; } = null;
        /// <summary>
        /// ClientUserReportFilter Detail.
        /// </summary>
        public GetClientReportFilterResponse ClientUserReportFilter { get; set; }
        /// <summary>
        /// EmployeeClient Detail.
        /// </summary>
        public EmployeeClientDto EmployeeClient { get; set; }
    }
}
