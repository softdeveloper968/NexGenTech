using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteRestoreClaimStatusBatch.Base;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using self_pay_eligibility_api.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteRestoreClaimStatusBatch.Queries
{
    public class GetAllClientInsuranceByBatchIdQuery : IRequest<Result<List<ClientInsuranceByBatchIdResponse>>>
    {
        public int TenantId { get; set; }
        public int ClaimStatusBatchId { get; set; }

        public GetAllClientInsuranceByBatchIdQuery(int tenantId, int claimStatusBatchId)
        {
            TenantId = tenantId;
            ClaimStatusBatchId = claimStatusBatchId;
        }
    }

    internal class GetAllClientInsuranceByBatchIdQueryHandler : IRequestHandler<GetAllClientInsuranceByBatchIdQuery, Result<List<ClientInsuranceByBatchIdResponse>>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetAllClientInsuranceByBatchIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<List<ClientInsuranceByBatchIdResponse>>> Handle(GetAllClientInsuranceByBatchIdQuery request, CancellationToken cancellationToken)
        {
            if (request.TenantId <= 0 || request.ClaimStatusBatchId <= 0)
            {
                return await Result<List<ClientInsuranceByBatchIdResponse>>.FailAsync("Invalid TenantId or ClaimStatusBatchId.");
            }

            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);
            var clientInsuranceRepository = _tenantRepositoryFactory.GetUnitOfWork<int>(request.TenantId).Repository<ClientInsurance>();

            var claimStatusBatch = await claimStatusRepo.Entities
                .FirstOrDefaultAsync(cl => cl.Id == request.ClaimStatusBatchId, cancellationToken);

            if (claimStatusBatch == null)
            {
                return await Result<List<ClientInsuranceByBatchIdResponse>>.FailAsync("Claim status batch not found.");
            }

            var clientInsuranceList = await clientInsuranceRepository.Entities
                .Where(c => c.ClientId == claimStatusBatch.ClientId)
                .Select(x => new ClientInsuranceByBatchIdResponse { Id = x.Id, Name = x.LookupName })
                .ToListAsync(cancellationToken);

            if (!clientInsuranceList.Any())
            {
                return await Result<List<ClientInsuranceByBatchIdResponse>>.FailAsync("No ClientInsurance found for this batch.");
            }

            return await Result<List<ClientInsuranceByBatchIdResponse>>.SuccessAsync(clientInsuranceList);
        }
    }

}
