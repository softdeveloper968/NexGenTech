using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class SystemDefaultReportFilterRepository : RepositoryAsync<SystemDefaultReportFilter, int>, ISystemDefaultReportFilterRepository
    {
        private readonly ApplicationContext _dbContext;
        public SystemDefaultReportFilterRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SystemDefaultReportFilter>> GetSystemDefaultReportFilterByReportIdAsync(ReportsEnum report)
        {
            return await _dbContext.SystemDefaultReportFilters.
                Include(s => s.SystemDefaultReportFilterEmployeeRoles)
                .Where(r => r.ReportId == report)
                .ToListAsync();
        }

        public async Task<List<SystemDefaultReportFilter>> GetSystemDefaultReportByEmployeeClientMatchedRoles(List<ClientEmployeeRole> assignedClientEmployeeRoles)
        {
            List<SystemDefaultReportFilter> systemDefaultReportFilters = await _dbContext.SystemDefaultReportFilters
                .Include(s => s.SystemDefaultReportFilterEmployeeRoles)
                .Where(x => x.SystemDefaultReportFilterEmployeeRoles
                .Any(y => assignedClientEmployeeRoles
                .Select(z => z.EmployeeRoleId).Contains(y.EmployeeRoleId)))
                .ToListAsync();
            return systemDefaultReportFilters;
        }
    }
}
