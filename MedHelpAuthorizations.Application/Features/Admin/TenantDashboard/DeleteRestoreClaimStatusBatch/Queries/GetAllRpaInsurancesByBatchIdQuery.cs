using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteRestoreClaimStatusBatch.Base;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using self_pay_eligibility_api.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteRestoreClaimStatusBatch.Queries
{
    public class GetAllRpaInsurancesByBatchIdQuery : IRequest<Result<List<RpaInsurancesByBatchIdResponse>>>
    {
        public int TenantId { get; set; }

        public GetAllRpaInsurancesByBatchIdQuery(int tenantId)
        {
            TenantId = tenantId;
        }
    }

    internal class GetAllRpaInsurancesByBatchIdQueryHandler : IRequestHandler<GetAllRpaInsurancesByBatchIdQuery, Result<List<RpaInsurancesByBatchIdResponse>>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetAllRpaInsurancesByBatchIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }

        public async Task<Result<List<RpaInsurancesByBatchIdResponse>>> Handle(GetAllRpaInsurancesByBatchIdQuery request, CancellationToken cancellationToken)
        {
            if (request.TenantId <= 0)
            {
                return await Result<List<RpaInsurancesByBatchIdResponse>>.FailAsync("Invalid TenantId");
            }

            var rpaInsuranceRepo = _tenantRepositoryFactory.GetUnitOfWork<int>(request.TenantId).Repository<RpaInsurance>();

            var rpaInsuranceList = await rpaInsuranceRepo.Entities
                .Where(c => c.InactivatedOn == null)
                .Select(x => new RpaInsurancesByBatchIdResponse { Id = x.Id, Code = x.Code })
                .ToListAsync(cancellationToken);

            if (!rpaInsuranceList.Any())
            {
                return await Result<List<RpaInsurancesByBatchIdResponse>>.FailAsync("No RpaInsurance found for this batch.");
            }

            return await Result<List<RpaInsurancesByBatchIdResponse>>.SuccessAsync(rpaInsuranceList);
        }
    }

}
