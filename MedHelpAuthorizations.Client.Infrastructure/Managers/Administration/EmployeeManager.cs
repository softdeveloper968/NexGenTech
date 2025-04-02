using MedHelpAuthorizations.Application.Features.Administration.Employees;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public EmployeeManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<PaginatedResult<GetAllPagedEmployeeResponse>> GetAllEmployeesPagedAsync(GetAllPagedEmployeesRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(EmployeesEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedEmployeeResponse>();
        }

        public async Task<IResult<List<EmployeeDto>>> GetAllManagers()
        {
            try
            {
                var manager = await _tenantHttpClient.GetAsync(EmployeesEndpoints.GetAllManagers());
                return await manager.ToResult<List<EmployeeDto>>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IResult<int>> SaveAsync(AddEditEmployeeCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(EmployeesEndpoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{EmployeesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<EmployeeDto>>> GetAllEmployees()
        {
            var manager = await _tenantHttpClient.GetAsync(EmployeesEndpoints.GetAllEmployees());
            return await manager.ToResult<List<EmployeeDto>>();
        }

        public async Task<IResult<EmployeeDto>> GetEmployeeByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(EmployeesEndpoints.GetEmployeeById(id));
            return await response.ToResult<EmployeeDto>();
        }
        public async Task<IResult<List<EmployeeDto>>> GetAllEmployeeData()
        {
            try
            {
                var response = await _tenantHttpClient.GetAsync(EmployeesEndpoints.GetAllEmployeeData);
                return await response.ToResult<List<EmployeeDto>>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
