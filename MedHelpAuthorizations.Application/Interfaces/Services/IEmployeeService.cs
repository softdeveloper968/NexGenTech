using MedHelpAuthorizations.Application.Features.Administration.Employees;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetEmployeeSearchQuery;
using MedHelpAuthorizations.Application.Interfaces.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IEmployeeService : IService
    {
        public Task<List<EmployeeDto>> GetEmployeeSearch(GetEmployeeSearchQuery employeeSearchOptions);
    }
}
