using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.GetAllPagedRpaInsurancesQuery.Models;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.TenantDashboard.GetAllPagedRpaInsurancesQuery.Queries
{
    public class GetAllPagedRpaInsurancesQuery : IRequest<PaginatedResult<GetAllPagedRpaInsurancesQueryResponse>>
    {
        public GetAllPagedRpaInsurancesQuery(int tenantId, int pageNumber, int pageSize, string quickSearch, string sortField, string sortOrder, bool isActiveOnly)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            QuickSearch = quickSearch;
            SortField = sortField;
            SortOrder = sortOrder;
            IsActiveOnly = isActiveOnly;
        }

        public int TenantId { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public bool IsActiveOnly { get; }
        public string QuickSearch { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
    }
    public class GetAllPagedRpaInsurancesQueryHandler : IRequestHandler<GetAllPagedRpaInsurancesQuery, PaginatedResult<GetAllPagedRpaInsurancesQueryResponse>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetAllPagedRpaInsurancesQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<PaginatedResult<GetAllPagedRpaInsurancesQueryResponse>> Handle(GetAllPagedRpaInsurancesQuery request, CancellationToken cancellationToken)
        {
            var rpaInsuranceRepository = _tenantRepositoryFactory.GetUnitOfWork<int>(request.TenantId).Repository<RpaInsurance>();

            var query = rpaInsuranceRepository.Entities;

            if (request.IsActiveOnly)
            {
                query = query.Where(x => x.InactivatedOn == null);
            }
            else
            {
                query = query.Where(x => x.InactivatedOn != null);
            }

            if (!string.IsNullOrEmpty(request.QuickSearch))
            {
                query = query.Where(x => x.Code.Contains(request.QuickSearch) || x.TargetUrl.Contains(request.QuickSearch) || x.RpaInsuranceGroup.Name.Contains(request.QuickSearch));
            }

            var res = await query.Select(x => new GetAllPagedRpaInsurancesQueryResponse()
            {
                Id = x.Id,
                RPACode = x.Code,
                TargetUrl = x.TargetUrl,
                ApprovalWaitPeriodDays = x.ApprovalWaitPeriodDays,
                ClaimBilledOnWaitDays = x.ClaimBilledOnWaitDays,
                DefaultTargetUrl = x.RpaInsuranceGroup.DefaultTargetUrl,
                GroupName = x.RpaInsuranceGroup.Name,
                InActivatedOn = x.InactivatedOn
            })
            .OrderByMappings(request.SortField, request.SortOrder)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return res;
        }
    }
}
