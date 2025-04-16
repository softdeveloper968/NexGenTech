using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteriaPaged
{
    public class GetByCriteriaPagedInsurancesQuery : IRequest<PaginatedResult<GetByCriteriaPagedInsurancesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int ClientId { get; set; }
        public GetByCriteriaPagedInsurancesQuery()
        {            
        }
    }

    public class GetByCriteriaInsurancesQueryHandler : IRequestHandler<GetByCriteriaPagedInsurancesQuery, PaginatedResult<GetByCriteriaPagedInsurancesResponse>>
    {
        private readonly IClientInsuranceRepository _clientInsuranceRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetByCriteriaInsurancesQueryHandler(IClientInsuranceRepository clientInsuranceRepository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _clientInsuranceRepository = clientInsuranceRepository;
            _mapper = mapper; 
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetByCriteriaPagedInsurancesResponse>> Handle(GetByCriteriaPagedInsurancesQuery request, CancellationToken cancellationToken)
        {
            request.ClientId = _clientId;
            return await _clientInsuranceRepository.GetByCriteria(request);            
        }
    }
}