using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Queries.GetAllPagedData
{
    public class GetAllPagedClientEncounterTypeQuery : IRequest<PaginatedResult<GetAllPagedClientEncounterTypesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetAllPagedClientEncounterTypeQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllPagedClientEncounterTypeQueryHandler : IRequestHandler<GetAllPagedClientEncounterTypeQuery, PaginatedResult<GetAllPagedClientEncounterTypesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllPagedClientEncounterTypeQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllPagedClientEncounterTypesResponse>> Handle(GetAllPagedClientEncounterTypeQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientEncounterType, GetAllPagedClientEncounterTypesResponse>> expression = e => new GetAllPagedClientEncounterTypesResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                clientId = e.ClientId
            };
            var data = await _unitOfWork.Repository<ClientEncounterType>().Entities
                .Where(x => x.ClientId == _clientId)
               .Select(expression)
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}
