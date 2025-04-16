using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetByCriteria
{
	public class GetClaimStatusBatchesByRpaInsuranceIdQuery : IRequest<Result<List<GetClaimStatusUnprocessedBatchesResponse>>>
	{
		public int RpaInsuranceId { get; set; }
		public bool IncludeAssignedBatches { get; set; }
		public bool IsForInitialAnalysis { get; set; } = false;
	}

	public class GetClaimStatusBatchesByRpaInsuranceIdQueryHandler : IRequestHandler<GetClaimStatusBatchesByRpaInsuranceIdQuery, Result<List<GetClaimStatusUnprocessedBatchesResponse>>>
	{
		private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
		private readonly IMapper _mapper;

		public GetClaimStatusBatchesByRpaInsuranceIdQueryHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IMapper mapper)
		{
			_claimStatusBatchRepository = claimStatusBatchRepository;
			_mapper = mapper;
		}

		public async Task<Result<List<GetClaimStatusUnprocessedBatchesResponse>>> Handle(GetClaimStatusBatchesByRpaInsuranceIdQuery query, CancellationToken cancellationToken)
		{
			var claimStatusBatchList = await _claimStatusBatchRepository.GetByRpaInsuranceId(query);
			var mappedClaimStatusBatches = _mapper.Map<List<GetClaimStatusUnprocessedBatchesResponse>>(claimStatusBatchList);
			return await Result<List<GetClaimStatusUnprocessedBatchesResponse>>.SuccessAsync(mappedClaimStatusBatches);
		}
	}
}
