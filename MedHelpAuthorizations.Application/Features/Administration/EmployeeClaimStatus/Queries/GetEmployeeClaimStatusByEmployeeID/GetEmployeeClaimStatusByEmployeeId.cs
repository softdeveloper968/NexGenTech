using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.EmployeeClaimStatus.Queries.GetEmployeeClaimStatusByEmployeeID
{
    public class GetEmployeeClaimStatusByEmployeeId : IRequest<Result<List<EmployeesClaimStatusResponseModel>>>
    {
        public int EmployeeId { get; set; }

        public GetEmployeeClaimStatusByEmployeeId()
        {
        }

        public class GetEmployeeClaimStatusExceptionReasonCategoriesByEmployeeIdQueryHandler : IRequestHandler<GetEmployeeClaimStatusByEmployeeId, Result<List<EmployeesClaimStatusResponseModel>>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;

            public GetEmployeeClaimStatusExceptionReasonCategoriesByEmployeeIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<Result<List<EmployeesClaimStatusResponseModel>>> Handle(GetEmployeeClaimStatusByEmployeeId request, CancellationToken cancellationToken)
            {
                Expression<Func<Domain.Entities.EmployeeClaimStatusExceptionReasonCategory, EmployeesClaimStatusResponseModel>> expression = e => _mapper.Map<EmployeesClaimStatusResponseModel>(e);
                var employeeDenialCriteriaSpec = new EmployeeClaimStatusExceptionReasonCategoriesSpecification(request.EmployeeId);

                var data = await _unitOfWork.Repository<Domain.Entities.EmployeeClaimStatusExceptionReasonCategory>().Entities
                   .Specify(employeeDenialCriteriaSpec)
                   .Select(expression)
                   .ToListAsync();

                return await Result<List<EmployeesClaimStatusResponseModel>>.SuccessAsync(data);
            }
        }
    }
}
