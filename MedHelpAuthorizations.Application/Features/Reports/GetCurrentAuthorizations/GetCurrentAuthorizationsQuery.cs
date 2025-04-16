using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Reports.GetCurrentAuthorizations
{
    public class GetPagedCurrentAuthorizationsQuery : IRequest<PaginatedResult<GetCurrentAuthorizationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int ClientId { get; set; }

        public GetPagedCurrentAuthorizationsQuery()
        {

        }
    }

    public class GetPagedCurrentAuthorizationsQueryHandler : IRequestHandler<GetPagedCurrentAuthorizationsQuery, PaginatedResult<GetCurrentAuthorizationsResponse>>
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetPagedCurrentAuthorizationsQueryHandler(IAuthorizationRepository authorizationRepository, ICurrentUserService currentUserService)
        {
            _authorizationRepository = authorizationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetCurrentAuthorizationsResponse>> Handle(GetPagedCurrentAuthorizationsQuery request, CancellationToken cancellationToken)
        {
            request.ClientId = _clientId;
            return await _authorizationRepository.GetCurrentWithProperNamePaginated(request);
        }
    }
}
