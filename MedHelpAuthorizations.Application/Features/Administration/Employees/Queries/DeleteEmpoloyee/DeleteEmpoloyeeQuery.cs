using MedHelpAuthorizations.Application.Exceptions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.DeleteEmpoloyee
{
    public class DeleteEmpoloyeeQuery : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public class DeleteEmpoloyeeQueryHandler : IRequestHandler<DeleteEmpoloyeeQuery, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;

            private readonly IEmployeeRepository _employeeRepository;
            public DeleteEmpoloyeeQueryHandler(IUnitOfWork<int> unitOfWork, IEmployeeRepository employeeRepository)
            {
                _employeeRepository = employeeRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result<int>> Handle(DeleteEmpoloyeeQuery command, CancellationToken cancellationToken)
            {
                try
                {
                    var employee = await _unitOfWork.Repository<Employee>().Entities
                        .Where(emp => emp.Id == command.Id)
                        .FirstOrDefaultAsync();
                    if (employee == null)
                        return await Result<int>.FailAsync($"Error Deleting Employee: Employeenot found)");
                    await _unitOfWork.Repository<Employee>().DeleteAsync(employee);

                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(employee.Id);
                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync($"Error Deleting EmployeeId = {command.Id}" + ex.Message + Environment.NewLine + ex.InnerException?.Message);
                }
            }
        }
    }
}
