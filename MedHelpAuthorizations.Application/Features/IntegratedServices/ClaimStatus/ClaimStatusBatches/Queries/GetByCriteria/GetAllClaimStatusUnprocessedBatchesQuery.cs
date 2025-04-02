using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria
{
	public class GetAllClaimStatusUnprocessedBatchesQuery : IRequest<Result<List<GetClaimStatusUnprocessedBatchesResponse>>>
	{
		public bool IsForInitialAnalysis { get; set; } = false;
	}

	public class GetAllClaimStatusUnprocessedBatchesQueryHandler : IRequestHandler<GetAllClaimStatusUnprocessedBatchesQuery, Result<List<GetClaimStatusUnprocessedBatchesResponse>>>
	{
		private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
		private readonly IMapper _mapper;

		public GetAllClaimStatusUnprocessedBatchesQueryHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IMapper mapper)
		{
			_claimStatusBatchRepository = claimStatusBatchRepository;
			_mapper = mapper;
		}

		public async Task<Result<List<GetClaimStatusUnprocessedBatchesResponse>>> Handle(GetAllClaimStatusUnprocessedBatchesQuery query, CancellationToken cancellationToken)
		{
			var claimStatusBatchList = await _claimStatusBatchRepository.GetAllUnprocessedAsync(query.IsForInitialAnalysis);

			var mappedClaimStatusBatches = _mapper.Map<List<GetClaimStatusUnprocessedBatchesResponse>>(claimStatusBatchList);

			return await Result<List<GetClaimStatusUnprocessedBatchesResponse>>.SuccessAsync(mappedClaimStatusBatches);
		}

	}
}
