using MedHelpAuthorizations.Application.Features.Administration.Employees;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IEmployeeManager : IManager
    {
        Task<PaginatedResult<GetAllPagedEmployeeResponse>> GetAllEmployeesPagedAsync(GetAllPagedEmployeesRequest request);
     //   Task<IResult<List<Employee>>> GetAllManager();
        Task<IResult<int>> SaveAsync(AddEditEmployeeCommand request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<EmployeeDto>>> GetAllEmployees();
        Task<IResult<List<EmployeeDto>>> GetAllManagers();
        Task<IResult<EmployeeDto>> GetEmployeeByIdAsync(int id);
        Task<IResult<List<EmployeeDto>>> GetAllEmployeeData();

	}
}
