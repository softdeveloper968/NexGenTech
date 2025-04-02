using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryBatches.Queries.GetById
{
    public class GetChargeEntryBatchByIdQuery : IRequest<Result<GetChargeEntryBatchByIdResponse>>
    {
        public int Id { get; set; }

        public class GetChargeEntryBatchByIdQueryHandler : IRequestHandler<GetChargeEntryBatchByIdQuery, Result<GetChargeEntryBatchByIdResponse>>
        {
            private readonly IChargeEntryBatchRepository _chargeEntryBatch;
            private readonly IMapper _mapper;

            public GetChargeEntryBatchByIdQueryHandler(IChargeEntryBatchRepository chargeEntryBatch, IMapper mapper)
            {
                _chargeEntryBatch = chargeEntryBatch;
                _mapper = mapper;
            }

            public async Task<Result<GetChargeEntryBatchByIdResponse>> Handle(GetChargeEntryBatchByIdQuery query, CancellationToken cancellationToken)
            {
                var chargeEntryBatch = await _chargeEntryBatch.GetByIdAsync(query.Id);
                var mappedChargeEntryBatch = _mapper.Map<GetChargeEntryBatchByIdResponse>(chargeEntryBatch);
                
                return await Result<GetChargeEntryBatchByIdResponse>.SuccessAsync(mappedChargeEntryBatch);
            }
        }
    }
}
