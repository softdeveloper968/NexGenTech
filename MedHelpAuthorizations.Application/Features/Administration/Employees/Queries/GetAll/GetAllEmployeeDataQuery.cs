using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetAll
{
	public class GetAllEmployeeDataQuery : IRequest<Result<List<EmployeeDto>>>
	{
	}
	public class GetAllEmployeeDataQueryHandler : IRequestHandler<GetAllEmployeeDataQuery, Result<List<EmployeeDto>>>
	{
		private readonly IUnitOfWork<int> _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public GetAllEmployeeDataQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_userService = userService;
		}

		public async Task<Result<List<EmployeeDto>>> Handle(GetAllEmployeeDataQuery request, CancellationToken cancellationToken)
		{
			var employeeDtos = await _unitOfWork.Repository<Employee>().Entities
				.Include(x => x.EmployeeManager)
				.Include(x => x.DefaultEmployeeRole)
				.Include(x => x.EmployeeClients)
                .Select(x => _mapper.Map<EmployeeDto>(x))
                .ToListAsync();

            foreach (var employee in employeeDtos) //AA-230
            {
                var user = await _userService.GetAsync(employee.UserId);

                //map employee user
                if (user.Data != null)
                {
                   // employee.User = user.Data;
                }

                //map employee manager user
                if (employee.EmployeeManager != null)
                {
					var manager = await _userService.GetAsync(employee.EmployeeManager.UserId);
					if (manager?.Data != null) 
					{
						//employee.EmployeeManager.User = user.Data;
					}
				}
            }
            return Result<List<EmployeeDto>>.Success(employeeDtos);
		}
	}
}
