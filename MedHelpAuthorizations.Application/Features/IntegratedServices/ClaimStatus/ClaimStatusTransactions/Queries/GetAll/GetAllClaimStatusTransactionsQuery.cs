using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Queries.GetAll
{
    public class GetAllClaimStatusTransactionsQuery : IRequest<Result<List<GetAllClaimStatusTransactionsResponse>>>
    {
        public GetAllClaimStatusTransactionsQuery()
        {
        }
    }

    public class GetAllClaimStatusTransactionsQueryHandler : IRequestHandler<GetAllClaimStatusTransactionsQuery, Result<List<GetAllClaimStatusTransactionsResponse>>>
    {
        private readonly IClaimStatusTransactionRepository _claimStatusTransaction;
        private readonly IMapper _mapper;

        public GetAllClaimStatusTransactionsQueryHandler(IClaimStatusTransactionRepository claimStatusTransaction, IMapper mapper)
        {
            _claimStatusTransaction = claimStatusTransaction;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllClaimStatusTransactionsResponse>>> Handle(GetAllClaimStatusTransactionsQuery request, CancellationToken cancellationToken)
        {
            var claimStatusTransactionList = await _claimStatusTransaction.GetListAsync();
            var mappedClaimStatusTransactions = _mapper.Map<List<GetAllClaimStatusTransactionsResponse>>(claimStatusTransactionList);
            return Result<List<GetAllClaimStatusTransactionsResponse>>.Success(mappedClaimStatusTransactions);
        }
    }
}
