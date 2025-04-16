using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged
{
    public class GetAllAuthorizationsQuery : IRequest<PaginatedResult<GetAllPagedAuthorizationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public int ClientId { get; set; }


        public GetAllAuthorizationsQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllAuthorizationsQueryHandler : IRequestHandler<GetAllAuthorizationsQuery, PaginatedResult<GetAllPagedAuthorizationsResponse>>
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllAuthorizationsQueryHandler(IAuthorizationRepository authorizationRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> Handle(GetAllAuthorizationsQuery request, CancellationToken cancellationToken)
        {
            request.ClientId = _clientId;
            return await _authorizationRepository.GetAllWithProperName(request);
        }
    }
}