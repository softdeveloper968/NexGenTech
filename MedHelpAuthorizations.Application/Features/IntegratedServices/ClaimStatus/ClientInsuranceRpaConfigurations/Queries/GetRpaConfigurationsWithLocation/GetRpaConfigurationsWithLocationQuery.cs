using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetRpaConfigurationsWithLocation
{
    public class GetRpaConfigurationsWithLocationQuery : IRequest<Result<List<GetRpaConfigurationsWithLocationResponse>>>
    {
        public GetRpaConfigurationsWithLocationQuery()
        {
        }
    }
    public class GetRpaConfigurationsWithLocationQueryHandler : IRequestHandler<GetRpaConfigurationsWithLocationQuery, Result<List<GetRpaConfigurationsWithLocationResponse>>>
    {
        private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetRpaConfigurationsWithLocationQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetRpaConfigurationsWithLocationResponse>>> Handle(GetRpaConfigurationsWithLocationQuery request, CancellationToken cancellationToken)
        {
            var clientInsuranceRpaConfigurationList = await _clientInsuranceRpaConfigurationRepository.GetRpaConfigurationsWithLocationAsync(_clientId);
            return Result<List<GetRpaConfigurationsWithLocationResponse>>.Success(clientInsuranceRpaConfigurationList);
        }
    }
}
