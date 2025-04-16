using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.AbortedClaimBatches.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.AbortedClaimBatches.Queries
{
    public class GetAbortedClaimBatchesQuery : IRequest<PaginatedResult<GetAbortedClaimBatchesQueryResponse>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string QuickSearch { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public GetAbortedClaimBatchesQuery(int tenantId, int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            QuickSearch = quickSearch;
            SortField = sortField;
            SortOrder = sortOrder;
        }
    }

    public class GetAbortedClaimBatchesQueryHandler : IRequestHandler<GetAbortedClaimBatchesQuery, PaginatedResult<GetAbortedClaimBatchesQueryResponse>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetAbortedClaimBatchesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<PaginatedResult<GetAbortedClaimBatchesQueryResponse>> Handle(GetAbortedClaimBatchesQuery request, CancellationToken cancellationToken)
        {
            var claimStatusRepo = _tenantRepositoryFactory.Get<IClaimStatusBatchRepository>(request.TenantId);

            var res = await claimStatusRepo.GetAbortedClaimStatusBatchesAsync(request.PageNumber, request.PageSize, request.QuickSearch, request.SortField, request.SortOrder);

            int index = (request.PageNumber - 1) * request.PageSize;

            var rows = res.Data.Select(x => new GetAbortedClaimBatchesQueryResponse()
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

            return PaginatedResult<GetAbortedClaimBatchesQueryResponse>.Success(rows, res.TotalRows, request.PageNumber, request.PageSize);
        }
    }
}
