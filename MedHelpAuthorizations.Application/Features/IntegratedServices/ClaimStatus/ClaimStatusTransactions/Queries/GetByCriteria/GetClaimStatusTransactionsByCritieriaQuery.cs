using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusTransactions.Queries.GetByCriteria
{
    public class GetClaimStatusTransactionsByCritieriaQuery: IRequest<Result<List<GetClaimStatusTransactionByCriteriaResponse>>>
    {
        public GetClaimStatusTransactionsByCritieriaQuery()
        {
        }
    }

    public class GetClaimStatusTransactionsByCritieriaQueryHandler : IRequestHandler<GetClaimStatusTransactionsByCritieriaQuery, Result<List<GetClaimStatusTransactionByCriteriaResponse>>>
    {
        private readonly IClaimStatusTransactionRepository _claimStatusTransaction;
        private readonly IMapper _mapper;

        public GetClaimStatusTransactionsByCritieriaQueryHandler(IClaimStatusTransactionRepository claimStatusTransaction, IMapper mapper)
        {
            _claimStatusTransaction = claimStatusTransaction;
            _mapper = mapper;
        }

        public async Task<Result<List<GetClaimStatusTransactionByCriteriaResponse>>> Handle(GetClaimStatusTransactionsByCritieriaQuery request, CancellationToken cancellationToken)
        {
            var claimStatusTransactionList = await _claimStatusTransaction.GetListAsync();
            var mappedClaimStatusTransactions = _mapper.Map<List<GetClaimStatusTransactionByCriteriaResponse>>(claimStatusTransactionList);
            return Result<List<GetClaimStatusTransactionByCriteriaResponse>>.Success(mappedClaimStatusTransactions);
        }

    }
}
