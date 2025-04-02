using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetByRpaType
{
    public class GetChargeEntryRpaConfigurationByRpaTypeQuery : IRequest<Result<List<GetChargeEntryRpaConfigurationByRpaTypeResponse>>>
    {
        public RpaTypeEnum RpaTypeId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }

        public class GetChargeEntryRpaConfigurationByRpaTypeQueryHandler : IRequestHandler<GetChargeEntryRpaConfigurationByRpaTypeQuery, Result<List<GetChargeEntryRpaConfigurationByRpaTypeResponse>>>
        {
            private readonly IChargeEntryRpaConfigurationRepository _chargeEntryRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public GetChargeEntryRpaConfigurationByRpaTypeQueryHandler(IChargeEntryRpaConfigurationRepository chargeEntryRpaConfigurationRepository, IMapper mapper)
            {
                _chargeEntryRpaConfigurationRepository = chargeEntryRpaConfigurationRepository;
                _mapper = mapper;
            }

            public async Task<Result<List<GetChargeEntryRpaConfigurationByRpaTypeResponse>>> Handle(GetChargeEntryRpaConfigurationByRpaTypeQuery query, CancellationToken cancellationToken)
            {
                var chargeEntryRpaConfigurationList = await _chargeEntryRpaConfigurationRepository.GetByRpaTypeIdAsync(query.RpaTypeId, query.TransactionTypeId);
                var mappedChargeEntryRpaConfigurations = _mapper.Map<List<GetChargeEntryRpaConfigurationByRpaTypeResponse>>(chargeEntryRpaConfigurationList);
                return Result<List<GetChargeEntryRpaConfigurationByRpaTypeResponse>>.Success(mappedChargeEntryRpaConfigurations);
            }
        }
    }
}
