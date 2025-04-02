using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetEmployeeSearchQuery;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class EmployeeRepository : RepositoryAsync<Employee, int>, IEmployeeRepository
	{
		private readonly ApplicationContext _dbContext;
		public EmployeeRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
        public IQueryable<Employee> Employees => _dbContext.Employees;

        public async Task<List<Employee>> GetEmployeeByDefaultEmployeeRole(int employeeRoleId)
		{
			return await Employees
								   .Where(x => x.DefaultEmployeeRoleId == (EmployeeRoleEnum)employeeRoleId)
								   .Include(x => x.DefaultEmployeeRole)
									.Include(x => x.EmployeeClients)
								   .ToListAsync();
		}

		public async Task<Employee> GetById(int employeeId)
		{
			return await Employees             
				.Include(x => x.EmployeeManager)
				.Include(x => x.DefaultEmployeeRole)
				.Include(x => x.EmployeeClients)
				.FirstOrDefaultAsync(x => x.Id == employeeId && x.IsDeleted == false);
		}

		public async Task<IEnumerable<Employee>> GetAllEmployees()
		{
			var employees = await Employees
				.Include(x => x.EmployeeManager)
				.Include(x => x.DefaultEmployeeRole)
				.Include(x => x.EmployeeClients)
			   .Where(x => x.IsDeleted == false)
					.ToListAsync();

			return employees;
		}

		public async Task<List<Employee>> GetEmployeeSearch(GetEmployeeSearchQuery employeeSearchOptions)
		{
			return await Employees
				.Where(e => e.IsDeleted == false).ToListAsync();

		}

		//MOve to its owen repository.. The method name would be justr GetByEmployeeId() because it is in the EmployeeClientInsuranceRepository that should ONLY GET EmployeeClientInsurances.
		public async Task<List<EmployeeClientInsurance>> GetInsurancesByEmployeeId(int employeeId)
		{
			return await _dbContext.EmployeeClientInsurances
										   .Where(x => x.EmployeeClientId == employeeId)
										   .Include(x => x.ClientInsurance)
										   .ToListAsync();
		}

		//MOve to its owen repository.. The method name would be justr GetByEmployeeId() because it is in the EmployeeClientLocationRepository that should ONLY GET EmployeeClientLocations.
		public async Task<List<EmployeeClientLocation>> GetLocationsByEmployeeId(int employeeId)
		{
			return await _dbContext.EmployeeClientLocations
										   .Where(x => x.EmployeeClientId == employeeId)
										   .Include(x => x.ClientLocation)
										   .ToListAsync();
		}

        //TODO: Move to the EmployeeClient repository
        public async Task DeleteEmployeeClientAsync(int employeeClientId) //AA-233
        {
            var employeeClient = await _dbContext.EmployeeClients
                .Include("EmployeeClientInsurances.ClientInsurance")
                .Include("EmployeeClientLocations.ClientLocation")
                .Include("AssignedClientEmployeeRoles.EmployeeRole")
                .Include("EmployeeClientAlphaSplits")
                .Where(ec => ec.Id == employeeClientId).FirstOrDefaultAsync();

			if (employeeClient != null)
			{
				_dbContext.ClientEmployeeRoles.RemoveRange(employeeClient.AssignedClientEmployeeRoles);
				_dbContext.EmployeeClientAlphaSplits.RemoveRange(employeeClient.EmployeeClientAlphaSplits);
				_dbContext.EmployeeClientInsurances.RemoveRange(employeeClient.EmployeeClientInsurances);
				_dbContext.EmployeeClientLocations.RemoveRange(employeeClient.EmployeeClientLocations);
			}

			await _dbContext.SaveChangesAsync();
		}

        /// <summary>
        /// Add new Employee for tenant
        /// </summary>
        /// <param name="newEmployee"></param>
        /// <returns></returns>
        public async Task<bool> AddEmployeeForTenantAsync(Employee newEmployee) //AA-216
        {
            try
            {
                var existingEmployee = Employees.Where(e => e.UserId == newEmployee.UserId).FirstOrDefault();
                if (existingEmployee != null)
                {
                    return false;
                }
                await _dbContext.Employees.AddAsync(newEmployee);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<Employee> GetEmployeeByUserId(string userId)
        {
            try
            {
                return await Employees
                   .Include(x => x.EmployeeClients)
                   .Where(x => x.UserId == userId)
                   .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

				throw;
			}
		}

		//TODO : Move to the EmployeeClient repository
		public async Task<List<EmployeeClient>> GetRegistorEmployeeClientByLocationIdAsync(int locationId, int clientId)
		{
			try
			{
				return await _dbContext.EmployeeClients
								.Include(ec => ec.EmployeeClientAlphaSplits)
								.Include(ec => ec.Employee)
								.Where(emp =>
								emp.ClientId == clientId
								&& emp.Client.ClientLocations.Any(ecl => ecl.EligibilityLocationId == locationId)
								&& emp.AssignedClientEmployeeRoles.Any(el => el.EmployeeRole.EmployeeRoleDepartments.Any(d => d.DepartmentId == DepartmentEnum.Registor))
								)
								.ToListAsync();
			}
			catch (Exception ex)
			{

				throw;
			}
		}

		//TODO : Move to the EmployeeClient repository
		public async Task<List<EmployeeClient>> GetAllBillingManagersOrFollowUpEmployeessByClientId(int clientId)
		{
			try
			{
				return await _dbContext.EmployeeClients
								.Include(ec => ec.Employee)
								.Where(emp =>
								emp.ClientId == clientId
								&& emp.AssignedClientEmployeeRoles.Any(x => x.EmployeeRole.EmployeeLevel == EmployeeLevelEnum.ManagerLevel
								&& x.EmployeeRole.EmployeeRoleDepartments.Any(x => x.DepartmentId == DepartmentEnum.Billing)))
								.ToListAsync();
			}
			catch (Exception ex)
			{

				throw;
			}
		}

	}
}