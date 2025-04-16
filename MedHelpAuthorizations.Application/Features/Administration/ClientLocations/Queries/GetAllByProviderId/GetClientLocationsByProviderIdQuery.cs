using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllByProviderId
{
    public class GetClientLocationsByProviderIdQuery : IRequest<Result<List<GetClientLocationsByProviderIdResponse>>>
    {
        public int ProviderId { get; set; }

        public GetClientLocationsByProviderIdQuery()
        {
        }
    }
    public class GetClientLocationsByProviderIdQueryHandler : IRequestHandler<GetClientLocationsByProviderIdQuery, Result<List<GetClientLocationsByProviderIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IClientProviderLocationRepository _providerLocationRepository;
        private int _clientId => _currentUserService.ClientId;

        public GetClientLocationsByProviderIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IClientProviderLocationRepository providerLocationRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _providerLocationRepository = providerLocationRepository;
        }

        public async Task<Result<List<GetClientLocationsByProviderIdResponse>>> Handle(GetClientLocationsByProviderIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientLocation, GetClientLocationsByProviderIdResponse>> expression = e => _mapper.Map<GetClientLocationsByProviderIdResponse>(e);

            var clientLocations = await _providerLocationRepository.GetClientLocationsByProviderId(request.ProviderId);
            List<GetClientLocationsByProviderIdResponse> result = _mapper.Map<List<GetClientLocationsByProviderIdResponse>>(clientLocations);
;
            return await Result<List<GetClientLocationsByProviderIdResponse>>.SuccessAsync(result);
        }
    }
}
