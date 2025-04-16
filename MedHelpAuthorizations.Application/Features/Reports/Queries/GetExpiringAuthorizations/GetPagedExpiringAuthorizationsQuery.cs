using AutoMapper;
using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;

using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.GetExpiringAuthorizations
{
    public class GetPagedExpiringAuthorizationsQuery : IRequest<PaginatedResult<GetAllPagedAuthorizationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int ClientId { get; set; }
        public DateTime RelativeDate { get; set; } = DateTime.UtcNow;
        public int ExpiringDays { get; set; } = 30;

        public GetPagedExpiringAuthorizationsQuery()
        {

        }
    }

    public class GetPagedExpiringAuthorizationsQueryHandler : IRequestHandler<GetPagedExpiringAuthorizationsQuery, PaginatedResult<GetAllPagedAuthorizationsResponse>>
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetPagedExpiringAuthorizationsQueryHandler(IAuthorizationRepository authorizationRepository, ICurrentUserService currentUserService)
        {
            _authorizationRepository = authorizationRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllPagedAuthorizationsResponse>> Handle(GetPagedExpiringAuthorizationsQuery request, CancellationToken cancellationToken)
        {
            request.ClientId = _clientId;
            return await _authorizationRepository.GetExpiringWithProperNamePaginated(request);
        }
    }
}
