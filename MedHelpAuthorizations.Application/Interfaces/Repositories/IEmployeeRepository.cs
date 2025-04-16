using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetEmployeeSearchQuery;
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository : IRepositoryAsync<Employee, int>
    {
        public Task<Employee> GetById(int employeeId);
        public Task<IEnumerable<Employee>> GetAllEmployees();   
        public Task<List<Employee>> GetEmployeeByDefaultEmployeeRole(int employeeRoleId);
        public Task<List<Employee>> GetEmployeeSearch(GetEmployeeSearchQuery employeeSearchOptions);
        public Task<List<EmployeeClientInsurance>> GetInsurancesByEmployeeId (int employeeId);   
        public Task<List<EmployeeClientLocation>> GetLocationsByEmployeeId (int employeeId);
        Task<bool> AddEmployeeForTenantAsync(Employee newEmployee); //AA-216
        Task<Employee> GetEmployeeByUserId(string userId); //AA-233
        Task DeleteEmployeeClientAsync(int employeeClientId); //AA-233

        Task<List<EmployeeClient>> GetRegistorEmployeeClientByLocationIdAsync(int locationId, int clientId);
        Task<List<EmployeeClient>> GetAllBillingManagersOrFollowUpEmployeessByClientId(int clientId);

    }
}
