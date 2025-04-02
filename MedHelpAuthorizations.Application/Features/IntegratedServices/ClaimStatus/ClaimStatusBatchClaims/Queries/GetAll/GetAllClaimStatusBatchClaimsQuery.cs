using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetAll
{
    public class GetAllClaimStatusBatchClaimsQuery : IRequest<Result<List<GetAllClaimStatusBatchClaimsResponse>>>
    {
    }

    public class GetAllClaimStatusBatchClaimsQueryHandler : IRequestHandler<GetAllClaimStatusBatchClaimsQuery, Result<List<GetAllClaimStatusBatchClaimsResponse>>>
    {
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IMapper _mapper;

        public GetAllClaimStatusBatchClaimsQueryHandler(IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IMapper mapper)
        {
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClaimStatusBatchClaimsResponse>>> Handle(GetAllClaimStatusBatchClaimsQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchClaimsList = await _claimStatusBatchClaimsRepository.GetAllAsync();
            var mappedClaimStatusBatchClaims = _mapper.Map<List<GetAllClaimStatusBatchClaimsResponse>>(claimStatusBatchClaimsList);
            return await Result<List<GetAllClaimStatusBatchClaimsResponse>>.SuccessAsync(mappedClaimStatusBatchClaims);
        }
    }
}
    