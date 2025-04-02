using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Base;
using MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Commands;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IEmployeeClientManager : IManager
    {
        Task<PaginatedResult<GetAllPagedEmployeeResponse>> GetAllEmployeeClientsPagedAsync(GetAllPagedEmployeesRequest request);
        Task<IResult<int>> SaveAsync(AddEditEmployeeClientCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        //Task<IResult<List<EmployeeClientDto>>> GetAllEmployeeClients();
        Task<IResult<EmployeeClientDto>> GetEmployeeClientByIdAsync(int id);

        Task<IResult<List<EmployeeClientViewModel>>> GetEmployeeClientViewModelsByClientIdAsync(int clientId);
    }
}
