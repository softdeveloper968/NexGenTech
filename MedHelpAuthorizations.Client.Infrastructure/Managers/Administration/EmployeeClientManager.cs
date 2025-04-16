using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Commands;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.Administration.Employees;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class EmployeeClientManager : IEmployeeClientManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public EmployeeClientManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{EmployeeClientsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        //public Task<IResult<List<EmployeeClientDto>>> GetAllEmployeeClients()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<PaginatedResult<GetAllPagedEmployeeResponse>> GetAllEmployeeClientsPagedAsync(GetAllPagedEmployeesRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(EmployeeClientsEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedEmployeeResponse>();
        }

        public async Task<IResult<EmployeeClientDto>> GetEmployeeClientByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(EmployeeClientsEndpoints.GetEmployeeClientById(id));
            return await response.ToResult<EmployeeClientDto>();
        }

        public async Task<IResult<List<EmployeeClientViewModel>>> GetEmployeeClientViewModelsByClientIdAsync(int clientId)
        {
            var response = await _tenantHttpClient.GetAsync(EmployeeClientsEndpoints.GetEmployeeClientViewModelsByClientId(clientId));
            return await response.ToResult<List<EmployeeClientViewModel>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditEmployeeClientCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(EmployeeClientsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
        
    }
}
