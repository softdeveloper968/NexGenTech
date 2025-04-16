using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAll
{
    public class GetAllAuthTypesQuery : IRequest<Result<List<GetAllPagedAuthTypesResponse>>>
    {
    }

    public class GetAllAuthTypesQueryHandler : IRequestHandler<GetAllAuthTypesQuery, Result<List<GetAllPagedAuthTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllAuthTypesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<GetAllPagedAuthTypesResponse>>> Handle(GetAllAuthTypesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<AuthType, GetAllPagedAuthTypesResponse>> expression = e => new GetAllPagedAuthTypesResponse
            {
                Id = e.Id,
                Name = e.Name,
            };
            var data = await _unitOfWork.Repository<AuthType>().Entities
               .Select(expression)
               .ToListAsync();
            return await Result<List<GetAllPagedAuthTypesResponse>>.SuccessAsync(data);
        }
    }
}
