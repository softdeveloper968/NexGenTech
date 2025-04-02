using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetAll
{
    public class GetAllClientInsuranceRpaConfigurationsQuery : IRequest<Result<List<GetAllClientInsuranceRpaConfigurationsResponse>>>
    {
        public GetAllClientInsuranceRpaConfigurationsQuery()
        {
        }
    }

    public class GetAllClientInsuranceRpaConfigurationsQueryHandler : IRequestHandler<GetAllClientInsuranceRpaConfigurationsQuery, Result<List<GetAllClientInsuranceRpaConfigurationsResponse>>>
    {
        private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllClientInsuranceRpaConfigurationsQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClientInsuranceRpaConfigurationsResponse>>> Handle(GetAllClientInsuranceRpaConfigurationsQuery request, CancellationToken cancellationToken)
        {
            var clientInsuranceRpaConfigurationList = await _clientInsuranceRpaConfigurationRepository.GetByClientIdAsync(_clientId);
            var mappedClientInsuranceRpaConfigurations = _mapper.Map<List<GetAllClientInsuranceRpaConfigurationsResponse>>(clientInsuranceRpaConfigurationList);
            return Result<List<GetAllClientInsuranceRpaConfigurationsResponse>>.Success(mappedClientInsuranceRpaConfigurations);
        }
    }
}
