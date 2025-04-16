using AutoMapper;
using MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Employees.Queries.GetManagerByEmployeeId
{
    public class GetManagerByCriteriaQuery : IRequest<Result<Employee>>
    {
        public int Id { get; set; }
    }

    public class GetManagerByEmployeeIdQueryHandler : IRequestHandler<GetManagerByCriteriaQuery, Result<Employee>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetManagerByEmployeeIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Employee>> Handle(GetManagerByCriteriaQuery query, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Repository<Domain.Entities.Employee>()
                .Entities
                .ToListAsync();

            var mappedEmployee = _mapper.Map<Employee>(employee); //Employee does not have reference to user so can't return user details.
            return await Result<Employee>.SuccessAsync(mappedEmployee);
        }
    }
}
