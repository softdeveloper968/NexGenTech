using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetAll
{
    public class GetAllChargeEntryBatchesQuery : IRequest<Result<List<GetAllChargeEntryBatchesResponse>>>
    {
        public GetAllChargeEntryBatchesQuery()
        {
        }
    }

    public class GetAllChargeEntryBatchesQueryHandler : IRequestHandler<GetAllChargeEntryBatchesQuery, Result<List<GetAllChargeEntryBatchesResponse>>>
    {
        private readonly IChargeEntryBatchRepository _chargeEntryBatch;
        private readonly IMapper _mapper;

        public GetAllChargeEntryBatchesQueryHandler(IChargeEntryBatchRepository chargeEntryBatch, IMapper mapper)
        {
            _chargeEntryBatch = chargeEntryBatch;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllChargeEntryBatchesResponse>>> Handle(GetAllChargeEntryBatchesQuery request, CancellationToken cancellationToken)
        {
            var chargeEntryBatchList = await this._chargeEntryBatch.GetListAsync().ConfigureAwait(true);
            var mappedChargeEntryBatches = _mapper.Map<List<GetAllChargeEntryBatchesResponse>>(chargeEntryBatchList);
            return await Result<List<GetAllChargeEntryBatchesResponse>>.SuccessAsync(mappedChargeEntryBatches).ConfigureAwait(true);
        }
    }
}
