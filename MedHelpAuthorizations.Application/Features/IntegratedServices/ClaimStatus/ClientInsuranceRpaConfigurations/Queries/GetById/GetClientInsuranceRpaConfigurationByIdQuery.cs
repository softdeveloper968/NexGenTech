using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetById
{
    public class GetClientInsuranceRpaConfigurationByIdQuery : IRequest<Result<GetClientInsuranceRpaConfigurationByIdResponse>>
    {
        public int Id { get; set; }

        public class GetClientInsuranceBotConfigurationByIdQueryHandler : IRequestHandler<GetClientInsuranceRpaConfigurationByIdQuery, Result<GetClientInsuranceRpaConfigurationByIdResponse>>
        {
            private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceBotConfiguration;
            private readonly IMapper _mapper;

            public GetClientInsuranceBotConfigurationByIdQueryHandler(IClientInsuranceRpaConfigurationRepository clientInsuranceBotConfiguration, IMapper mapper)
            {
                _clientInsuranceBotConfiguration = clientInsuranceBotConfiguration;
                _mapper = mapper;
            }

            public async Task<Result<GetClientInsuranceRpaConfigurationByIdResponse>> Handle(GetClientInsuranceRpaConfigurationByIdQuery query, CancellationToken cancellationToken)
            {
                var clientInsuranceBotConfiguration = await _clientInsuranceBotConfiguration.GetByIdAsync(query.Id);
                var mappedClientInsuranceBotConfiguration = _mapper.Map<GetClientInsuranceRpaConfigurationByIdResponse>(clientInsuranceBotConfiguration);
                return Result<GetClientInsuranceRpaConfigurationByIdResponse>.Success(mappedClientInsuranceBotConfiguration);
            }
        }
    }
}
