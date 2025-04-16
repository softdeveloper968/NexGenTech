using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetFailed
{
    public class GetFailedClientInsuranceRpaConfigurationsQuery : IRequest<Result<List<GetFailedClientInsuranceRpaConfigurationsResponse>>>
    {
        public class GetFailedClientInsuranceRpaConfigurationsQueryyHandler : IRequestHandler<GetFailedClientInsuranceRpaConfigurationsQuery, Result<List<GetFailedClientInsuranceRpaConfigurationsResponse>>>
        {
            private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public GetFailedClientInsuranceRpaConfigurationsQueryyHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IMapper mapper)
            {
                _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<GetFailedClientInsuranceRpaConfigurationsResponse>>> Handle(GetFailedClientInsuranceRpaConfigurationsQuery query, CancellationToken cancellationToken)
            {
                var clientInsuranceRpaConfigurationList = await _clientInsuranceRpaConfigurationRepository.GetFailedClientInsuranceRpaConfigurationsAsync();
                var mappedClientInsuranceRpaConfigurations = _mapper.Map<List<GetFailedClientInsuranceRpaConfigurationsResponse>>(clientInsuranceRpaConfigurationList);
                return await  Result<List<GetFailedClientInsuranceRpaConfigurationsResponse>>.SuccessAsync(mappedClientInsuranceRpaConfigurations);
            }
        }
    }
}
