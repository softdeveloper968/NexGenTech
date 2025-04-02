using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetProcedureCodes
{
    public class GetClientClaimStatusProcedureCodesQuery : IRequest<Result<List<GetClaimStatusClientProcedureCodeResponse>>>
    {
    }

    public class GetClientClaimStatusProcedureCodesQueryHandler : IRequestHandler<GetClientClaimStatusProcedureCodesQuery, Result<List<GetClaimStatusClientProcedureCodeResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetClientClaimStatusProcedureCodesQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetClaimStatusClientProcedureCodeResponse>>> Handle(GetClientClaimStatusProcedureCodesQuery query, CancellationToken cancellationToken)
        {
            var claimStatusBatchClaimsList = await _unitOfWork.Repository<ClaimStatusBatchClaim>()
                .Entities
                .Specify(new ClaimStatusClaimByClientIdFilterSpecification(_clientId))
                .GroupBy(x => x.ProcedureCode.Substring(0, 5).Trim())
                .OrderByDescending(x => x.Key)
                .Select(y => new GetClaimStatusClientProcedureCodeResponse() { ProcedureCode = y.Key})
                .ToListAsync();
            return await Result<List<GetClaimStatusClientProcedureCodeResponse>>.SuccessAsync(claimStatusBatchClaimsList);
        }
    }
}
