using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetUnresolvedByBatchId
{
    public class GetQualifiedUnresolvedBatchClaimsByBatchIdQuery : IRequest<Result<List<GetQualifiedUnresolvedBatchClaimsByBatchIdResponse>>>
    {
        public int ClaimStatusBatchId { get; set; }
        public int ReturnQuantityCap { get; set; } = 100000;
    }

    public class GetQualifiedUnresolvedBatchClaimsByBatchIdQueryHandler : IRequestHandler<GetQualifiedUnresolvedBatchClaimsByBatchIdQuery, Result<List<GetQualifiedUnresolvedBatchClaimsByBatchIdResponse>>>
    {
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IUnitOfWork<int> _unitOfWork;
        //private readonly IClaimStatusBatchRepository _claimStatusBatchRepository;
        private readonly IMapper _mapper;

        public GetQualifiedUnresolvedBatchClaimsByBatchIdQueryHandler(IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetQualifiedUnresolvedBatchClaimsByBatchIdResponse>>> Handle(GetQualifiedUnresolvedBatchClaimsByBatchIdQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchClaimsList = await _claimStatusBatchClaimsRepository.GetUnresolvedByBatchIdAsync(query.ClaimStatusBatchId, query.ReturnQuantityCap);

            if(!claimStatusBatchClaimsList.Any())
            {
                var notExpiredOrFullyUnresolvedClaimCount = await _claimStatusBatchClaimsRepository.GetUnresolvedCountByBatchIdAsync(query.ClaimStatusBatchId);
                if(notExpiredOrFullyUnresolvedClaimCount == 0)
                {
                    _unitOfWork.Repository<ClaimStatusBatch>().ExecuteUpdate(b => b.Id == query.ClaimStatusBatchId,
                    u => 
                    { 
                        u.AllClaimStatusesResolvedOrExpired = true; 
                    });
                }
            }

            var mappedClaimStatusBatchClaims = _mapper.Map<List<GetQualifiedUnresolvedBatchClaimsByBatchIdResponse>>(claimStatusBatchClaimsList);
            return await Result<List<GetQualifiedUnresolvedBatchClaimsByBatchIdResponse>>.SuccessAsync(mappedClaimStatusBatchClaims);
        }
    }
}
