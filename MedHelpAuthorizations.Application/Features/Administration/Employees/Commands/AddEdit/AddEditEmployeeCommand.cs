using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Commands.AddEdit
{
    public class AddEditEmployeeCommand : EmployeeDto, IRequest<Result<int>>
    {

    }

    public class AddEditEmployeeCommandHandler : IRequestHandler<AddEditEmployeeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<AddEditClientCommandHandler> _localizer;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;


        public AddEditEmployeeCommandHandler(IUnitOfWork<int> unitOfWork, IUserService userService, IMapper mapper, IMediator mediator, IStringLocalizer<AddEditClientCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
            _localizer = localizer;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditEmployeeCommand command, CancellationToken cancellationToken)
        {
            Employee dbEmployee = null;
            //var person = await _unitOfWork.Repository<Person>().Entities.FirstOrDefaultAsync(x => x.Email == command.Email);
            if (command.Id == 0 || command.Id == null)
            {
                try
                {
                    //insert employee
                    //if (command.EmployeeManagerId == 0) { command.EmployeeManagerId = null; }
                    var employee = _mapper.Map<Employee>(command);
                    await _unitOfWork.Repository<Employee>().AddAsync(employee);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(dbEmployee.Id, _localizer["Employee Created"]);

                }
                catch (Exception ex)
                {
                    return await Result<int>.FailAsync(_localizer["Employee Not Found!"]);
                    throw;
                }
            }
            else
            {
                dbEmployee = await _unitOfWork.Repository<Employee>().GetByIdAsync((int)command.Id);

                if (dbEmployee != null)
                {
                    try
                    {
                        //update employee
                        var employee = _mapper.Map<Employee>(command);
                        await _unitOfWork.Repository<Employee>().UpdateAsync(employee);

                        if (command.UpdateReportReceiveForAllClients == true) //AA-289
                        { 
                            // Execute the bulk update using your specific library's ExecuteUpdate method
                            _unitOfWork.Repository<EmployeeClient>().ExecuteUpdate(p => p.EmployeeId == dbEmployee.Id,
                                ec =>
                                {
                                    ec.ReceiveReport = command.DefaultReceiveReport;
                                }
                            );
                        }

                        //commit changes
                        await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(dbEmployee.Id, _localizer["Employee Updated"]);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                else //no employee or person exists
                {
                    return await Result<int>.FailAsync(_localizer["Employee Not Found!"]);
                }
            }
        }
    }
}
