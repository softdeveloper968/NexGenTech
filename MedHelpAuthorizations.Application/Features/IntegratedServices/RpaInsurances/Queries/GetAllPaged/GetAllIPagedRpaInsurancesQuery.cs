using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Specifications;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Queries.GetAll;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.RpaInsurances.Queries.GetAllPaged
{
    public class GetAllPagedRpaInsurancesQuery : IRequest<PaginatedResult<GetAllRpaInsurancesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetAllPagedRpaInsurancesQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllRpaInsurancesQueryHandler : IRequestHandler<GetAllPagedRpaInsurancesQuery, PaginatedResult<GetAllRpaInsurancesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public GetAllRpaInsurancesQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllRpaInsurancesResponse>> Handle(GetAllPagedRpaInsurancesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<RpaInsurance, GetAllRpaInsurancesResponse>> expression = e => new GetAllRpaInsurancesResponse
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name
            };

            var data = await _unitOfWork.Repository<RpaInsurance>().Entities
               .Specify(new GenericNameSearchSpecification<RpaInsurance>(request.SearchString))
			   .OrderBy(x => x.Name)
			   .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}