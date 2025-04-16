using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatches.Queries.GetById
{
    public class GetClaimStatusBatchByIdQuery : IRequest<Result<GetClaimStatusBatchByIdResponse>>
    {
        public int Id { get; set; }

        public class GetClaimStatusBatchByIdQueryHandler : IRequestHandler<GetClaimStatusBatchByIdQuery, Result<GetClaimStatusBatchByIdResponse>>
        {
            private readonly IClaimStatusBatchRepository _claimStatusBatch;
            private readonly IMapper _mapper;

            public GetClaimStatusBatchByIdQueryHandler(IClaimStatusBatchRepository claimStatusBatch, IMapper mapper)
            {
                _claimStatusBatch = claimStatusBatch;
                _mapper = mapper;
            }

            public async Task<Result<GetClaimStatusBatchByIdResponse>> Handle(GetClaimStatusBatchByIdQuery query, CancellationToken cancellationToken)
            {
                var claimStatusBatch = await _claimStatusBatch.GetByIdAsync(query.Id);
                var mappedClaimStatusBatch = _mapper.Map<GetClaimStatusBatchByIdResponse>(claimStatusBatch);
                //mappedClaimStatusBatch.BatchAssignmentDates = claimStatusBatch.ClaimStatusBatchHistories
                //    .Where(bh => bh.AssignedDateTimeUtc != null)
                //    .DistinctBy(bh => bh.AssignedDateTimeUtc)
                //    .Select(b => (DateTime)b.AssignedDateTimeUtc)
                //    .OrderByDescending(ad => ad)
                //    .ToList();

                return Result<GetClaimStatusBatchByIdResponse>.Success(mappedClaimStatusBatch);
            }
        }
    }
}
