using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria
{
    public class GetClaimStatusUnprocessedBatchesQuery : IRequest<Result<List<GetClaimStatusUnprocessedBatchesResponse>>>
    {
        public int RpaInsuranceId { get; set; }
        public bool IsForInitialAnalysis { get; set; } = false;
	}

    public class GetClaimStatusUnprocessedBatchesQueryHandler : IRequestHandler<GetClaimStatusUnprocessedBatchesQuery, Result<List<GetClaimStatusUnprocessedBatchesResponse>>>
    {
        private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
        private readonly IMapper _mapper;

        public GetClaimStatusUnprocessedBatchesQueryHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IMapper mapper)
        {
            _claimStatusBatchRepository = claimStatusBatchRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetClaimStatusUnprocessedBatchesResponse>>> Handle(GetClaimStatusUnprocessedBatchesQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchList = await _claimStatusBatchRepository.GetUnprocessedByCriteriaAsync(query);
            var mappedClaimStatusBatches = _mapper.Map<List<GetClaimStatusUnprocessedBatchesResponse>>(claimStatusBatchList);
            //mappedClaimStatusBatches = mappedClaimStatusBatches.OrderBy(x => x.BatchAssignmentCount).ToList();
            return await Result<List<GetClaimStatusUnprocessedBatchesResponse>>.SuccessAsync(mappedClaimStatusBatches);
        }
    }
}
