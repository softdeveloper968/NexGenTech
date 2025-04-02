using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface ISystemDefaultReportFilterRepository : IRepositoryAsync<SystemDefaultReportFilter, int>
    {
        Task<List<SystemDefaultReportFilter>> GetSystemDefaultReportFilterByReportIdAsync(ReportsEnum report);
        Task<List<SystemDefaultReportFilter>> GetSystemDefaultReportByEmployeeClientMatchedRoles(List<ClientEmployeeRole> assignedClientEmployeeRoles);
    }
}
