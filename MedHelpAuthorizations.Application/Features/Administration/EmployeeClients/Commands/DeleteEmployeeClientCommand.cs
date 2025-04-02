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

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClients.Commands
{
    public class DeleteEmployeeClientCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public class DeleteEmployeeClientCommandHandler : IRequestHandler<DeleteEmployeeClientCommand, Result<int>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;

            private readonly IEmployeeRepository _employeeRepository;
            public DeleteEmployeeClientCommandHandler(IUnitOfWork<int> unitOfWork, IEmployeeRepository employeeRepository)
            {
                _employeeRepository = employeeRepository;
                _unitOfWork = unitOfWork;
            }
            public async Task<Result<int>> Handle(DeleteEmployeeClientCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var employeeClient = await _unitOfWork.Repository<EmployeeClient>().Entities
                        .Include("EmployeeClientInsurances.ClientInsurance")
                        .Include("EmployeeClientLocations.ClientLocation")
                        .Include("AssignedClientEmployeeRoles.EmployeeRole")
                        .Include("EmployeeClientAlphaSplits")
                        .Where(emp => emp.Id == command.Id)
                        .FirstOrDefaultAsync();

                    if (employeeClient == null)
                        return await Result<int>.FailAsync($"Error Deleting EmployeeClientId = {command.Id}: EmployeeClient not found)");

                    _unitOfWork.Repository<ClientEmployeeRole>().RemoveRange(employeeClient.AssignedClientEmployeeRoles);
                    _unitOfWork.Repository<EmployeeClientAlphaSplit>().RemoveRange(employeeClient.EmployeeClientAlphaSplits);
                    _unitOfWork.Repository<EmployeeClientInsurance>().RemoveRange(employeeClient.EmployeeClientInsurances);
                    _unitOfWork.Repository<EmployeeClientLocation>().RemoveRange(employeeClient.EmployeeClientLocations);

                    await _unitOfWork.Repository<EmployeeClient>().DeleteAsync(employeeClient);
                    await _unitOfWork.Commit(cancellationToken);

                    return await Result<int>.SuccessAsync(employeeClient.Id);
                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync($"Error Deleting EmployeeClientId = {command.Id}" + ex.Message + Environment.NewLine + ex.InnerException?.Message);
                }
            }
        }
    }
}
