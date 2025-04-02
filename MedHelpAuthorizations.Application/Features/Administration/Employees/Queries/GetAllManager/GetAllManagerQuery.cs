using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAllManager
{
    
    public class GetAllManagersQuery : IRequest<Result<List<EmployeeDto>>>    
    {
    }

    public class GetAllManagerByQueryHandler : IRequestHandler<GetAllManagersQuery, Result<List<EmployeeDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllManagerByQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }        

        public async Task<Result<List<EmployeeDto>>> Handle(GetAllManagersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Employee, Employee>> expression = e => new Employee
            {
                Id = e.Id,
            };

            var data = await _unitOfWork.Repository<Employee>().Entities
                .Where(e => e.DefaultEmployeeRole.EmployeeLevel != EmployeeLevelEnum.NonManagementLevel)
                .ToListAsync();

            var managers = _mapper.Map<List<EmployeeDto>>(data);
            foreach (var manager in managers)
            {
                var user = await _userManager.FindByIdAsync(manager.UserId); //AA-206
                manager.User = user;
            }
            return Result<List<EmployeeDto>>.Success(managers);
        }
    }
}
