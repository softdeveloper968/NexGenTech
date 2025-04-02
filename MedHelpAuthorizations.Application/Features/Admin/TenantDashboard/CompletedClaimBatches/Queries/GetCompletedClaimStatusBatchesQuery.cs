using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.AbortedClaimBatches.Models;
using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.CompletedClaimBatches.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.CompletedClaimBatches.Queries
{
    public class GetCompletedClaimStatusBatchesQuery : IRequest<PaginatedResult<GetCompletedClaimStatusBatchesQueryResponse>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public GetCompletedClaimStatusBatchesQuery(int tenantId, int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            QuickSearch = quickSearch;
            SortField = sortField;
            SortOrder = sortOrder;
        }
    }
    public class GetCompletedClaimStatusBatchesQueryHandler : IRequestHandler<GetCompletedClaimStatusBatchesQuery, PaginatedResult<GetCompletedClaimStatusBatchesQueryResponse>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        public GetCompletedClaimStatusBatchesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<PaginatedResult<GetCompletedClaimStatusBatchesQueryResponse>> Handle(GetCompletedClaimStatusBatchesQuery request, CancellationToken cancellationToken)
        {
            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);

            var res = await claimStatusRepo.GetCompletedClaimStatusBatchesAsync(request.PageNumber, request.PageSize, request.QuickSearch, request.SortField, request.SortOrder);

            var rows = res.Data.Select(x => new GetCompletedClaimStatusBatchesQueryResponse()
            {
                Id = x.Id,
                AllClaimStatusesResolvedOrExpired = x.AllClaimStatusesResolvedOrExpired,
                AssignedDateTimeUtc = x.AssignedDateTimeUtc,
                AssignedToHostName = x.AssignedToHostName,
                AssignedToIpAddress = x.AssignedToIpAddress,
                AssignedToRpaLocalProcessIds = x.AssignedToRpaLocalProcessIds,
                BatchNumber = x.BatchNumber,
                ClientCode = x.ClientCode,
                CompletedDateTimeUtc = x.CompletedDateTimeUtc,
                CreatedOn = x.CreatedOn,
                LastModifiedOn = x.LastModifiedOn,
                LineItems = x.LineItems,
                Payer = x.Payer,
                RPA = x.RPA,
            }).ToList();

            return PaginatedResult<GetCompletedClaimStatusBatchesQueryResponse>.Success(rows, res.TotalRows, request.PageNumber, request.PageSize);
        }
    }
}
