using AutoMapper;
using MedHelpAuthorizations.Application.Enums;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged
{
    public class GetByCriteriaPagedAuthorizationsQuery : IRequest<PaginatedResult<GetByCriteriaPagedAuthorizationsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public AuthTypeNameFilterType AuthTypeNamesFilterType { get; set; } = AuthTypeNameFilterType.ThreeLetter;
        public string[] AuthTypeNames { get; set; }
        public AuthorizationStatusEnum[] AuthorizationStatuses { get; set; }
        public int ClientId { get; set; }
        public QueryStateTypeEnum QueryStateType { get; set; }
        public GetByCriteriaPagedAuthorizationsQuery()
        {            
        }
    }

    public class GetByCriteriaAuthorizationsQueryHandler : IRequestHandler<GetByCriteriaPagedAuthorizationsQuery, PaginatedResult<GetByCriteriaPagedAuthorizationsResponse>>
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetByCriteriaAuthorizationsQueryHandler(IAuthorizationRepository authorizationRepository , IMapper mapper, ICurrentUserService currentUserService)
        {
            _authorizationRepository = authorizationRepository;
            _mapper = mapper; 
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetByCriteriaPagedAuthorizationsResponse>> Handle(GetByCriteriaPagedAuthorizationsQuery request, CancellationToken cancellationToken)
        {
            request.ClientId = _clientId;
            return await _authorizationRepository.GetByCriteriaWithProperName(request);            
        }
    }
}