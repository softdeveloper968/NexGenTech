using MedHelpAuthorizations.Application.Features.Reports.ClientReportFilters;
using MedHelpAuthorizations.Application.Interfaces.Common;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IClientReportFilterService : IService
    {
        Task<List<GetClientReportFilterResponse>> GetClientReportFilterDetailsByClientId(int clientId);
        Task<List<GetClientReportFilterResponse>> GetClientReportFiltersByReportId(int reportId);
        Task<List<GetClientReportFilterResponse>> GetClientReportFiltersByReportIdAndFilterName(int reportId, string filterName);
        Task<ClientUserReportFilter> GetClientReportFilters(int reportFilterId, int clientId, ReportsEnum reportId);
        Task<ClientCustomReportFilterDetails> GetCustomReportClientReportFiltersByReportId(int reportId);
        Task<bool> AddSystemDefaultReportFiltersForEmployeeClient(int employeeClientId, string tenantIdentifier = null);
        /// <summary>
        /// Soft Delete Saved ClientUserReport By Id.
        /// </summary>
        /// <param name="clientUserReportId"> ClientUSerReport Id</param>
        /// <param name="tenantIdentifier">TenantName <see cref="| If Any"/></param>
        /// <returns>Success: ClientUserReportId and Message | Fail:Error Message</returns>
        Task<Result<int>> DeleteById(int clientUserReportId, string tenantIdentifier = null);
    }
}
