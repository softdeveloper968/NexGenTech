using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetById
{
    public class GetEmployeeByIdQuery : IRequest<Result<EmployeeDto>>
    {
        public int Id { get; set; }
    }

    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetEmployeeByIdQueryHandler(IEmployeeRepository employeeRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetById(query.Id);
                                    
                                    
            var mappedEmployee = _mapper.Map<EmployeeDto>(employee);
            var user = await _userManager.FindByIdAsync(mappedEmployee.UserId) ?? new(); //AA-206
            mappedEmployee.User = user;
            return await Result<EmployeeDto>.SuccessAsync(mappedEmployee);
        }
    }
}
