using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ReferringProviders.Queries.GetAllReferringProviders
{
    public class GetAllPagedReferringProvidersQuery : IRequest<PaginatedResult<GetAllReferringProvidersResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public GetAllPagedReferringProvidersQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllPagedReferringProvidersQueryHandler : IRequestHandler<GetAllPagedReferringProvidersQuery, PaginatedResult<GetAllReferringProvidersResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllPagedReferringProvidersQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllReferringProvidersResponse>> Handle(GetAllPagedReferringProvidersQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ReferringProvider, GetAllReferringProvidersResponse>> expression = e => new GetAllReferringProvidersResponse
            {
                //Id = e.Id,
            };
            //var patientFilterSpec = new PatientFilterSpecification(request.SearchString, _clientId);
            var data = await _unitOfWork.Repository<ReferringProvider>().Entities
                .Include(x => x.Person)
                .ThenInclude(x => x.Address)
                //.Specify(patientFilterSpec)
                .Select(expression)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}
