using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged
{
    public class GetPagedAuthTypesQuery : IRequest<PaginatedResult<GetAllPagedAuthTypesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetPagedAuthTypesQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetPagedAuthTypesQueryHandler : IRequestHandler<GetPagedAuthTypesQuery, PaginatedResult<GetAllPagedAuthTypesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetPagedAuthTypesQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedAuthTypesResponse>> Handle(GetPagedAuthTypesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<AuthType, GetAllPagedAuthTypesResponse>> expression = e => new GetAllPagedAuthTypesResponse
            {
                Id = e.Id,
                Name = e.Name,
            };
            var data = await _unitOfWork.Repository<AuthType>().Entities
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}