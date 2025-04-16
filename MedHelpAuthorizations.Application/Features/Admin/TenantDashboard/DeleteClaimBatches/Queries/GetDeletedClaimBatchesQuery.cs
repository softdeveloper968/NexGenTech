using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.AbortedClaimBatches.Models;
using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteClaimBatches.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.DeleteClaimBatches.Queries
{
    public class GetDeletedClaimBatchesQuery : IRequest<PaginatedResult<GetDeletedClaimBatchesQueryResponse>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public GetDeletedClaimBatchesQuery(int tenantId, int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            QuickSearch = quickSearch;
            SortField = sortField;
            SortOrder = sortOrder;
        }
    }
    public class GetDeletedClaimBatchesQueryHandler : IRequestHandler<GetDeletedClaimBatchesQuery, PaginatedResult<GetDeletedClaimBatchesQueryResponse>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        public GetDeletedClaimBatchesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<PaginatedResult<GetDeletedClaimBatchesQueryResponse>> Handle(GetDeletedClaimBatchesQuery request, CancellationToken cancellationToken)
        {
            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);

            var res = await claimStatusRepo.GetDeletedClaimStatusBatchesAsync(request.PageNumber, request.PageSize, request.QuickSearch, request.SortField, request.SortOrder);

            int index = (request.PageNumber - 1) * request.PageSize;

            var rows = res.Data.Select(x => new GetDeletedClaimBatchesQueryResponse()
            {
                Id = x.Id,
                AbortedOnUtc = x.AbortedOnUtc,
                AbortedReason = x.AbortedReason,
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

            return PaginatedResult<GetDeletedClaimBatchesQueryResponse>.Success(rows, res.TotalRows, request.PageNumber, request.PageSize);
        }
    }
}
