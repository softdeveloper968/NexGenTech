using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetEmployeeSearchQuery
{
    public class GetEmployeeSearchQuery : EmployeeSearchOptions, IRequest<Result<List<EmployeeDto>>>
    {
       
    }

    public class GetEmployeeSearchQueryHandler : IRequestHandler<GetEmployeeSearchQuery, Result<List<EmployeeDto>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public GetEmployeeSearchQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IEmployeeService employeeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<Result<List<EmployeeDto>>> Handle(GetEmployeeSearchQuery request, CancellationToken cancellationToken)
        {
                var employeeData = await _employeeService.GetEmployeeSearch(request);
                var mappedemployee = _mapper.Map<List<EmployeeDto>>(employeeData);
                return await Result<List<EmployeeDto>>.SuccessAsync(mappedemployee);
            
        }
    }
}
