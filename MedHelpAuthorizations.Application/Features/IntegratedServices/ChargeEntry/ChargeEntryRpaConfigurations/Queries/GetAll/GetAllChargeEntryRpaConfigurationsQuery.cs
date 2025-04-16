using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetAll
{
    public class GetAllChargeEntryRpaConfigurationsQuery : IRequest<Result<List<GetAllChargeEntryRpaConfigurationsResponse>>>
    {
        public GetAllChargeEntryRpaConfigurationsQuery()
        {
        }
    }

    public class GetAllChargeEntryRpaConfigurationsQueryHandler : IRequestHandler<GetAllChargeEntryRpaConfigurationsQuery, Result<List<GetAllChargeEntryRpaConfigurationsResponse>>>
    {
        private readonly IChargeEntryRpaConfigurationRepository _chargeEntryRpaConfigurationRepository;
        private readonly IMapper _mapper;

        public GetAllChargeEntryRpaConfigurationsQueryHandler(IChargeEntryRpaConfigurationRepository chargeEntryRpaConfigurationRepository, IMapper mapper)
        {
            _chargeEntryRpaConfigurationRepository = chargeEntryRpaConfigurationRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllChargeEntryRpaConfigurationsResponse>>> Handle(GetAllChargeEntryRpaConfigurationsQuery request, CancellationToken cancellationToken)
        {
            var chargeEntryRpaConfigurationList = await _chargeEntryRpaConfigurationRepository.GetListAsync();
            var mappedChargeEntryRpaConfigurations = _mapper.Map<List<GetAllChargeEntryRpaConfigurationsResponse>>(chargeEntryRpaConfigurationList);
            return Result<List<GetAllChargeEntryRpaConfigurationsResponse>>.Success(mappedChargeEntryRpaConfigurations);
        }
    }
}
