using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.Departments.Queries.GetDepartmentByClientId
{
    public class GetAllDepartment : IRequest<Result<List<Department>>>
    {
    }

    public class GetAllDepartmentQueryHandler : IRequestHandler<GetAllDepartment, Result<List<Department>>>
    {
        private readonly IUnitOfWork<DepartmentEnum> _unitOfWork;

        public GetAllDepartmentQueryHandler(IUnitOfWork<DepartmentEnum> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<Department>>> Handle(GetAllDepartment request, CancellationToken cancellationToken)
        {
            Expression<Func<Department, Department>> expression = e => new Department
            {
                Id = e.Id,
            };

            var data = await _unitOfWork.Repository<Department>().Entities
                                               .ToListAsync();
            return Result<List<Department>>.Success(data);
        }
    }
}

