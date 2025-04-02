using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ChargeEntry.ChargeEntryTransactions.Queries.GetAll
{
    public class GetAllChargeEntryTransactionsQuery : IRequest<Result<List<GetAllChargeEntryTransactionsResponse>>>
    {
        public GetAllChargeEntryTransactionsQuery()
        {
        }
    }

    public class GetAllChargeEntryTransactionsQueryHandler : IRequestHandler<GetAllChargeEntryTransactionsQuery, Result<List<GetAllChargeEntryTransactionsResponse>>>
    {
        private readonly IChargeEntryTransactionRepository _chargeEntryTransaction;
        private readonly IMapper _mapper;

        public GetAllChargeEntryTransactionsQueryHandler(IChargeEntryTransactionRepository chargeEntryTransaction, IMapper mapper)
        {
            _chargeEntryTransaction = chargeEntryTransaction;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllChargeEntryTransactionsResponse>>> Handle(GetAllChargeEntryTransactionsQuery request, CancellationToken cancellationToken)
        {
            var chargeEntryTransactionList = await _chargeEntryTransaction.GetListAsync();
            var mappedChargeEntryTransactions = _mapper.Map<List<GetAllChargeEntryTransactionsResponse>>(chargeEntryTransactionList);
            return await Result<List<GetAllChargeEntryTransactionsResponse>>.SuccessAsync(mappedChargeEntryTransactions);
        }
    }
}
