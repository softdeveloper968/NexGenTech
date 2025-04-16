using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.ClaimStatusBatchesHistories.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.ClaimStatusBatchesHistories.Queries
{
    public class GetClaimStatusBatchesHistoriesQuery : IRequest<PaginatedResult<GetClaimStatusBatchesHistoriesQueryResponse>>
    {
        public int TenantId { get; set; }
        public int ClaimStatusBatchId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
    }

    public class GetClaimStatusBatchesHistoriesQueryHandler : IRequestHandler<GetClaimStatusBatchesHistoriesQuery, PaginatedResult<GetClaimStatusBatchesHistoriesQueryResponse>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetClaimStatusBatchesHistoriesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<PaginatedResult<GetClaimStatusBatchesHistoriesQueryResponse>> Handle(GetClaimStatusBatchesHistoriesQuery request, CancellationToken cancellationToken)
        {
            var repo = _tenantRepositoryFactory.Get<IClaimStatusBatchHistoryRepository>(request.TenantId);

            var retVal = await repo.ClaimStatusBatchHistories
                .Where(x => x.ClaimStatusBatchId == request.ClaimStatusBatchId)
                .Select(x => new GetClaimStatusBatchesHistoriesQueryResponse
                {
                    Id = x.Id,
                    AbortedOnUtc = x.AbortedOnUtc,
                    AbortedReason = x.AbortedReason,
                    AllClaimStatusesResolvedOrExpired = x.AllClaimStatusesResolvedOrExpired,
                    AssignedClientRpaConfigurationId = x.AssignedClientRpaConfigurationId,
                    AssignedDateTimeUtc = x.AssignedDateTimeUtc,
                    AssignedToHostName = x.AssignedToHostName,
                    AssignedToIpAddress = x.AssignedToIpAddress,
                    AssignedToRpaCode = x.AssignedToRpaCode,
                    AssignedToRpaLocalProcessIds = x.AssignedToRpaLocalProcessIds,
                    AuthTypeId = x.AuthTypeId,
                    ClaimStatusBatchId = x.ClaimStatusBatchId,
                    ClientId = x.ClientId,
                    ClientInsuranceId = x.ClientInsuranceId,
                    CompletedDateTimeUtc = x.CompletedDateTimeUtc,
                    ReviewedBy = x.ReviewedBy,
                    ReviewedOnUtc = x.ReviewedOnUtc,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    LastModifiedBy = x.LastModifiedBy,
                    LastModifiedOn = x.LastModifiedOn
                })
                .OrderByMappings(request.SortField, request.SortOrder)
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return retVal;
        }
    }
}
