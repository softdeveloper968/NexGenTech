using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryRpaConfigurations.Queries.GetByCriteria
{
    public class GetSingleChargeEntryRpaConfigurationByCriteriaQuery : IRequest<Result<GetChargeEntryRpaConfigurationByCriteriaResponse>>
    {
        public int ClientId { get; set; }
        public RpaTypeEnum RpaTypeId { get; set; }
        public TransactionTypeEnum TransactionTypeId { get; set; }
        public int AuthTypeId { get; set; }

        public class GetChargeEntryRpaConfigurationByIdQueryHandler : IRequestHandler<GetSingleChargeEntryRpaConfigurationByCriteriaQuery, Result<GetChargeEntryRpaConfigurationByCriteriaResponse>>
        {
            private readonly IChargeEntryRpaConfigurationRepository _chargeEntryRpaConfigurationRepository;
            private readonly IMapper _mapper;

            public GetChargeEntryRpaConfigurationByIdQueryHandler(IChargeEntryRpaConfigurationRepository chargeEntryRpaConfigurationRepository, IMapper mapper)
            {
                _chargeEntryRpaConfigurationRepository = chargeEntryRpaConfigurationRepository;
                _mapper = mapper;
            }

            public async Task<Result<GetChargeEntryRpaConfigurationByCriteriaResponse>> Handle(GetSingleChargeEntryRpaConfigurationByCriteriaQuery query, CancellationToken cancellationToken)
            {
                var chargeEntryRpaConfigurationRpaConfiguration = await _chargeEntryRpaConfigurationRepository.GetByCriteriaAsync(query.ClientId, query.AuthTypeId, query.RpaTypeId, query.TransactionTypeId = TransactionTypeEnum.ChargeEntry);
                var mappedChargeEntryRpaConfiguration = _mapper.Map<GetChargeEntryRpaConfigurationByCriteriaResponse>(chargeEntryRpaConfigurationRpaConfiguration);
                return Result<GetChargeEntryRpaConfigurationByCriteriaResponse>.Success(mappedChargeEntryRpaConfiguration);
            }
        }
    }
}
