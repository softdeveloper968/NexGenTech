using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetAll
{
    public class GetAllClaimStatusBatchesQuery : IRequest<Result<List<GetAllClaimStatusBatchesResponse>>>
    {
        public GetAllClaimStatusBatchesQuery()
        {
        }
    }

    public class GetAllClaimStatusBatchesQueryHandler : IRequestHandler<GetAllClaimStatusBatchesQuery, Result<List<GetAllClaimStatusBatchesResponse>>>
    {
        private readonly IClaimStatusBatchRepository _claimStatusBatch;
        private readonly IMapper _mapper;

        public GetAllClaimStatusBatchesQueryHandler(IClaimStatusBatchRepository claimStatusBatch, IMapper mapper)
        {
            _claimStatusBatch = claimStatusBatch;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClaimStatusBatchesResponse>>> Handle(GetAllClaimStatusBatchesQuery request, CancellationToken cancellationToken)
        {
            var claimStatusBatchList = await _claimStatusBatch.GetListAsync();
            var mappedClaimStatusBatches = _mapper.Map<List<GetAllClaimStatusBatchesResponse>>(claimStatusBatchList);
            mappedClaimStatusBatches = mappedClaimStatusBatches.OrderBy(x => x.BatchAssignmentCount).ToList();
            return await Result<List<GetAllClaimStatusBatchesResponse>>.SuccessAsync(mappedClaimStatusBatches);
        }
    }
}
