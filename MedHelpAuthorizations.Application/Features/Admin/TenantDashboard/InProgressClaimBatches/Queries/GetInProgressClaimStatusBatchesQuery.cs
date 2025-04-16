using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.CompletedClaimBatches.Models;
using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.InProgressClaimBatches.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.InProgressClaimBatches.Queries
{
    public class GetInProgressClaimStatusBatchesQuery : IRequest<PaginatedResult<GetInProgressClaimStatusBatchesQueryResponse>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public GetInProgressClaimStatusBatchesQuery(int tenantId, int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            QuickSearch = quickSearch;
            SortField = sortField;
            SortOrder = sortOrder;
        }
    }

    public class GetInProgressClaimStatusBatchesQueryHandler : IRequestHandler<GetInProgressClaimStatusBatchesQuery, PaginatedResult<GetInProgressClaimStatusBatchesQueryResponse>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        public GetInProgressClaimStatusBatchesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<PaginatedResult<GetInProgressClaimStatusBatchesQueryResponse>> Handle(GetInProgressClaimStatusBatchesQuery request, CancellationToken cancellationToken)
        {
            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);

            var res = await claimStatusRepo.GetInProgressClaimStatusBatchesAsync(request.PageNumber, request.PageSize, request.QuickSearch, request.SortField, request.SortOrder);

            var rows = res.Data.Select(x => new GetInProgressClaimStatusBatchesQueryResponse()
            {
                Id = x.Id,
                AssignedDateTimeUtc = x.AssignedDateTimeUtc,
                AssignedToHostName = x.AssignedToHostName,
                AssignedToIpAddress = x.AssignedToIpAddress,
                AssignedToRpaLocalProcessIds = x.AssignedToRpaLocalProcessIds,
                BatchNumber = x.BatchNumber,
                ClientCode = x.ClientCode,
                CreatedOn = x.CreatedOn,
                LastModifiedOn = x.LastModifiedOn,
                LineItems = x.LineItems,
                Payer = x.Payer,
                RPA = x.RPA,
            }).ToList();

            return PaginatedResult<GetInProgressClaimStatusBatchesQueryResponse>.Success(rows, res.TotalRows, request.PageNumber, request.PageSize);
        }
    }
}
