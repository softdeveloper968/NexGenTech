using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetByCriteria
{
    public class GetAllChargeEntryUnprocessedBatchesQuery : IRequest<Result<List<GetChargeEntryUnprocessedBatchesResponse>>>
    {
    }

    public class GetAllChargeEntryUnprocessedBatchesQueryHandler : IRequestHandler<GetAllChargeEntryUnprocessedBatchesQuery, Result<List<GetChargeEntryUnprocessedBatchesResponse>>>
    {
        private readonly IChargeEntryBatchRepository _claimStatusBatchRepository;
        private readonly IMapper _mapper;

        public GetAllChargeEntryUnprocessedBatchesQueryHandler(IChargeEntryBatchRepository claimStatusBatchRepository, IMapper mapper)
        {
            _claimStatusBatchRepository = claimStatusBatchRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetChargeEntryUnprocessedBatchesResponse>>> Handle(GetAllChargeEntryUnprocessedBatchesQuery query, CancellationToken cancellationToken)
        {
            var chargeEntryBatchList = await _claimStatusBatchRepository.GetAllUnprocessedAsync();
            var mappedChargeEntryBatches = _mapper.Map<List<GetChargeEntryUnprocessedBatchesResponse>>(chargeEntryBatchList);
            return Result<List<GetChargeEntryUnprocessedBatchesResponse>>.Success(mappedChargeEntryBatches);
        }
    }
}
