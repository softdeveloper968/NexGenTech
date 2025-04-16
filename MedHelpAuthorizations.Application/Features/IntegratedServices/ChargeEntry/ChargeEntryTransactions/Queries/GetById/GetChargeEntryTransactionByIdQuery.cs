using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetById
{
    public class GetChargeEntryTransactionByIdQuery : IRequest<Result<GetChargeEntryTransactionByIdResponse>>
    {
        public int Id { get; set; }

        public class GetChargeEntryTransactionByIdQueryHandler : IRequestHandler<GetChargeEntryTransactionByIdQuery, Result<GetChargeEntryTransactionByIdResponse>>
        {
            private readonly IChargeEntryTransactionRepository _chargeEntryTransactionCache;
            private readonly IMapper _mapper;

            public GetChargeEntryTransactionByIdQueryHandler(IChargeEntryTransactionRepository chargeEntryTransactionCache, IMapper mapper)
            {
                _chargeEntryTransactionCache = chargeEntryTransactionCache;
                _mapper = mapper;
            }

            public async Task<Result<GetChargeEntryTransactionByIdResponse>> Handle(GetChargeEntryTransactionByIdQuery query, CancellationToken cancellationToken)
            {
                var chargeEntryTransaction = await _chargeEntryTransactionCache.GetByIdAsync(query.Id);
                var mappedChargeEntryTransaction = _mapper.Map<GetChargeEntryTransactionByIdResponse>(chargeEntryTransaction);
                return Result<GetChargeEntryTransactionByIdResponse>.Success(mappedChargeEntryTransaction);
            }
        }
    }
}
