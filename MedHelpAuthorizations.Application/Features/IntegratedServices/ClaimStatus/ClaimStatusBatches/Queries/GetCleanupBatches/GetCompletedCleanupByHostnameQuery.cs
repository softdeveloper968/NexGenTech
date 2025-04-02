using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetCleanupBatches
{
    public class GetCompletedCleanupByHostnameQuery : IRequest<Result<List<GetCompletedCleanupByHostnameResponse>>>
    {
        public string HostName = string.Empty;
    }

    public class GetCompletedCleanupByHostnameQueryHandler : IRequestHandler<GetCompletedCleanupByHostnameQuery, Result<List<GetCompletedCleanupByHostnameResponse>>>
    {
        private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
        private readonly IMapper _mapper;

        public GetCompletedCleanupByHostnameQueryHandler(IClaimStatusBatchRepository claimStatusBatchRepository, IMapper mapper)
        {
            _claimStatusBatchRepository = claimStatusBatchRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetCompletedCleanupByHostnameResponse>>> Handle(GetCompletedCleanupByHostnameQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchList = await _claimStatusBatchRepository.GetCompletedCleanupByHostName(query.HostName);
            var mappedClaimStatusBatches = _mapper.Map<List<GetCompletedCleanupByHostnameResponse>>(claimStatusBatchList);
            //mappedClaimStatusBatches = mappedClaimStatusBatches.OrderBy(x => x.BatchAssignmentCount).ToList();
            return await Result<List<GetCompletedCleanupByHostnameResponse>>.SuccessAsync(mappedClaimStatusBatches);
        }
    }
}
