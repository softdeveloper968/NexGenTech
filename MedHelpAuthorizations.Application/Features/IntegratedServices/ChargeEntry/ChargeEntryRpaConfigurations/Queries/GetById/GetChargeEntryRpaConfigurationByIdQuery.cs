using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetById
{
    public class GetChargeEntryRpaConfigurationByIdQuery : IRequest<Result<GetChargeEntryRpaConfigurationByIdResponse>>
    {
        public int Id { get; set; }

        public class GetChargeEntryBotConfigurationByIdQueryHandler : IRequestHandler<GetChargeEntryRpaConfigurationByIdQuery, Result<GetChargeEntryRpaConfigurationByIdResponse>>
        {
            private readonly IChargeEntryRpaConfigurationRepository _clientInsuranceBotConfiguration;
            private readonly IMapper _mapper;

            public GetChargeEntryBotConfigurationByIdQueryHandler(IChargeEntryRpaConfigurationRepository clientInsuranceBotConfiguration, IMapper mapper)
            {
                _clientInsuranceBotConfiguration = clientInsuranceBotConfiguration;
                _mapper = mapper;
            }

            public async Task<Result<GetChargeEntryRpaConfigurationByIdResponse>> Handle(GetChargeEntryRpaConfigurationByIdQuery query, CancellationToken cancellationToken)
            {
                var clientInsuranceBotConfiguration = await _clientInsuranceBotConfiguration.GetByIdAsync(query.Id);
                var mappedChargeEntryBotConfiguration = _mapper.Map<GetChargeEntryRpaConfigurationByIdResponse>(clientInsuranceBotConfiguration);
                return Result<GetChargeEntryRpaConfigurationByIdResponse>.Success(mappedChargeEntryBotConfiguration);
            }
        }
    }
}
