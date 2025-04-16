using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAllPaged
{
    public class GetAllEmployeeQuery : IRequest<PaginatedResult<EmployeeDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllEmployeeQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeeQuery, PaginatedResult<EmployeeDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetAllEmployeesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PaginatedResult<EmployeeDto>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
        {
            
            var employees = await _unitOfWork.Repository<Employee>().Entities
                .Include(x => x.EmployeeManager)
                .Include(x => x.DefaultEmployeeRole)
                .Include(x => x.EmployeeClients)
                .Where(e => e.IsDeleted == false) //AA-233
                .Select(x => _mapper.Map<EmployeeDto>(x))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            //if employees count is greater than 0 then find user for each userId and if user 
            //found then map the properties to the employeeDto
            if (employees.TotalCount > 0)
            {
                foreach (var employee in employees.Data)
                {
                    var user = await _userManager.FindByIdAsync(employee.UserId);
                    if (user != null)
                    {
                        employee.User = user;
                    }
                    var manager = await _userManager.FindByIdAsync(employee.EmployeeManager?.UserId);
                    if (manager != null) 
                    {
                        employee.EmployeeManager.User = manager;
                    }
                }
            }
            return employees;
        }
    }
}
