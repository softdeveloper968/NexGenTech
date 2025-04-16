using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed
{
    public class GetErrorOrFailedClientInsuranceRpaConfigByClientIdQuery : IRequest<Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>>
    {
        public class GetErrorOrFailedClientInsuranceRpaConfigByClientIdQueryHandler : IRequestHandler<GetErrorOrFailedClientInsuranceRpaConfigByClientIdQuery, Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>>
        {
            private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;
            private int _clientId => _currentUserService.ClientId;
            public GetErrorOrFailedClientInsuranceRpaConfigByClientIdQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IMapper mapper, ICurrentUserService currentUserService)
            {
                _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }

            public async Task<Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>> Handle(GetErrorOrFailedClientInsuranceRpaConfigByClientIdQuery query, CancellationToken cancellationToken)
            {
                var clientInsuranceRpaConfigurationList = await _clientInsuranceRpaConfigurationRepository.GetExpiryWarningOrFailedClientInsuranceRpaConfigByClientIdAsync(_clientId);
                //clientInsuranceRpaConfigurationList.Select();
                var mappedClientInsuranceRpaConfigurations = _mapper.Map<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>(clientInsuranceRpaConfigurationList);
                return await Result<List<GetErrorOrFailedClientInsuranceRpaConfigByClientIdResponse>>.SuccessAsync(mappedClientInsuranceRpaConfigurations);
            }
        }
    }
}
