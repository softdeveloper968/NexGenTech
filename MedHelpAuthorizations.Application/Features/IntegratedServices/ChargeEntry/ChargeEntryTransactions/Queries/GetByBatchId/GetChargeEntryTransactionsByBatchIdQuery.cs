using AutoMapper;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetByBatchId;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetById
{
    public class GetChargeEntryTransactionsByBatchIdQuery : IRequest<Result<GetChargeEntryTransactionsByBatchIdResponse>>
    {
        [Required]
        public int chargeEntryBatchId { get; set; }

        public class GetChargeEntryTransactionsByBatchIdQueryHandler : IRequestHandler<GetChargeEntryTransactionsByBatchIdQuery, Result<GetChargeEntryTransactionsByBatchIdResponse>>
        {
            private readonly IChargeEntryTransactionRepository _chargeEntryTransactionRepository;
            private readonly IMapper _mapper;

            public GetChargeEntryTransactionsByBatchIdQueryHandler(IChargeEntryTransactionRepository chargeEntryTransactionRepository, IMapper mapper)
            {
                _chargeEntryTransactionRepository = chargeEntryTransactionRepository;
                _mapper = mapper;
            }

            public async Task<Result<GetChargeEntryTransactionsByBatchIdResponse>> Handle(GetChargeEntryTransactionsByBatchIdQuery query, CancellationToken cancellationToken)
            {
                var chargeEntryTransaction = await _chargeEntryTransactionRepository.GetByBatchIdAsync(query.chargeEntryBatchId);
                var mappedChargeEntryTransaction = _mapper.Map<GetChargeEntryTransactionsByBatchIdResponse>(chargeEntryTransaction);
                
                return await Result<GetChargeEntryTransactionsByBatchIdResponse>.SuccessAsync(mappedChargeEntryTransaction);
            }
        }
    }
}
