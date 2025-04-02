using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetByCriteria
{
    public class GetChargeEntryUnprocessedBatchesQuery : IRequest<Result<List<GetChargeEntryUnprocessedBatchesResponse>>>
    {
    }

    public class GetChargeEntryUnprocessedBatchesQueryHandler : IRequestHandler<GetChargeEntryUnprocessedBatchesQuery, Result<List<GetChargeEntryUnprocessedBatchesResponse>>>
    {
        private readonly IChargeEntryBatchRepository _chargeEntryBatchRepository;
        private readonly IMapper _mapper;

        public GetChargeEntryUnprocessedBatchesQueryHandler(IChargeEntryBatchRepository chargeEntryBatchRepository, IMapper mapper)
        {
            _chargeEntryBatchRepository = chargeEntryBatchRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetChargeEntryUnprocessedBatchesResponse>>> Handle(GetChargeEntryUnprocessedBatchesQuery query, CancellationToken cancellationToken)
        {
            var chargeEntryBatchList = await _chargeEntryBatchRepository.GetAllUnprocessedAsync();
            var mappedChargeEntryBatches = _mapper.Map<List<GetChargeEntryUnprocessedBatchesResponse>>(chargeEntryBatchList);
            return await Result<List<GetChargeEntryUnprocessedBatchesResponse>>.SuccessAsync(mappedChargeEntryBatches);
        }
    }
}
