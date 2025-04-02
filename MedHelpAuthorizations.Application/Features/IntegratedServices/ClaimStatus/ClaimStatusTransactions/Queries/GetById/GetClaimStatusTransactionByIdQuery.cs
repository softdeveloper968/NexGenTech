using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Queries.GetById
{
    public class GetClaimStatusTransactionByIdQuery : IRequest<Result<GetClaimStatusTransactionByIdResponse>>
    {
        public int Id { get; set; }

        public class GetclaimStatusTransactionByIdQueryHandler : IRequestHandler<GetClaimStatusTransactionByIdQuery, Result<GetClaimStatusTransactionByIdResponse>>
        {
            private readonly IClaimStatusTransactionRepository _claimStatusTransactionCache;
            private readonly IMapper _mapper;

            public GetclaimStatusTransactionByIdQueryHandler(IClaimStatusTransactionRepository claimStatusTransactionCache, IMapper mapper)
            {
                _claimStatusTransactionCache = claimStatusTransactionCache;
                _mapper = mapper;
            }

            public async Task<Result<GetClaimStatusTransactionByIdResponse>> Handle(GetClaimStatusTransactionByIdQuery query, CancellationToken cancellationToken)
            {
                var claimStatusTransaction = await _claimStatusTransactionCache.GetByIdAsync(query.Id);
                var mappedClaimStatusTransaction = _mapper.Map<GetClaimStatusTransactionByIdResponse>(claimStatusTransaction);
                return Result<GetClaimStatusTransactionByIdResponse>.Success(mappedClaimStatusTransaction);
            }
        }
    }
}
