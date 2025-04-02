using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClaimStatusBatchClaims.Queries.GetProcedureCodes;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteRestoreClaimStatusBatch.Queries
{
    public class GetAllProcedureCodesByBatchIdQuery : IRequest<Result<List<GetClaimStatusClientProcedureCodeResponse>>>
    {
        public int TenantId { get; set; }
        public int ClaimStatusBatchId { get; set; }

        public GetAllProcedureCodesByBatchIdQuery(int tenantId, int claimStatusBatchId)
        {
            TenantId = tenantId;
            ClaimStatusBatchId = claimStatusBatchId;
        }
    }

    internal class GetAllProcedureCodesByBatchIdQueryHandler : IRequestHandler<GetAllProcedureCodesByBatchIdQuery, Result<List<GetClaimStatusClientProcedureCodeResponse>>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetAllProcedureCodesByBatchIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<List<GetClaimStatusClientProcedureCodeResponse>>> Handle(GetAllProcedureCodesByBatchIdQuery request, CancellationToken cancellationToken)
        {
            if (request.TenantId <= 0 || request.ClaimStatusBatchId <= 0)
            {
                return await Result<List<GetClaimStatusClientProcedureCodeResponse>>.FailAsync("Invalid TenantId or ClaimStatusBatchId.");
            }

            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);
            var cptCodeRepository = _tenantRepositoryFactory.Get<IClientCptCodeRepository>(request.TenantId);

            var claimStatusBatch = await claimStatusRepo.Entities
                .FirstOrDefaultAsync(cl => cl.Id == request.ClaimStatusBatchId, cancellationToken);

            if (claimStatusBatch == null)
            {
                return await Result<List<GetClaimStatusClientProcedureCodeResponse>>.FailAsync("Claim status batch not found.");
            }

            var procedureCodes = await cptCodeRepository.Entities
                .Where(c => c.ClientId == claimStatusBatch.ClientId)
                .Select(x => new GetClaimStatusClientProcedureCodeResponse { ProcedureCode = x.Code })
                .ToListAsync(cancellationToken);

            if (!procedureCodes.Any())
            {
                return await Result<List<GetClaimStatusClientProcedureCodeResponse>>.FailAsync("No procedure codes found for this batch.");
            }

            return await Result<List<GetClaimStatusClientProcedureCodeResponse>>.SuccessAsync(procedureCodes);
        }
    }

}
