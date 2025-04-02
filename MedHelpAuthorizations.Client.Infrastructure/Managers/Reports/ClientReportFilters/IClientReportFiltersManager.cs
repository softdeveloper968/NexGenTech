using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.ClientReportFilters
{
    public interface IClientReportFiltersManager : IManager
    {
        Task<string> SaveUpdateClientReportFilters(AddEditClientReportFiltersCommand query);
        Task<IResult<List<GetClientReportFilterResponse>>> GetClientReportFiltersByReportId(ReportsEnum report);
        Task<IResult<List<GetClientReportFilterResponse>>> GetClientReportFiltersByReportIdAndFilterName(ReportsEnum report, string filterName);
        Task<IResult<List<GetClientReportFilterResponse>>> GetClientReportFilterDetailsByClientId();
        Task<IResult<int>> DeleteClientReportFilterById(int id);
        Task<IResult<int>> DeleteSharedClientReportFilterById(int employeeClientReportFilterId);
        Task<IResult<ClientCustomReportFilterDetails>> GetCustomClientReportFiltersByReportId(ReportsEnum report);
        Task<string> SaveCustomClientReportFilters(AddEmployeeClientUserReportFilterCommand query);
    }
}
