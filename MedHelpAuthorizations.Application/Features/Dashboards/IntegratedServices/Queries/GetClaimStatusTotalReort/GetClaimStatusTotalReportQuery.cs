using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetClaimStatusTotalReort
{
    public class GetClaimStatusTotalReportQuery : IRequest<Result<List<GetClaimStatusTotalReportResponse>>>
    {
        public GetClaimStatusTotalReportQuery() { }
    }

    public class GetClaimStatusTotalReportQueryHandler : IRequestHandler<GetClaimStatusTotalReportQuery, Result<List<GetClaimStatusTotalReportResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetClaimStatusTotalReportQueryHandler> _localizer;
        private readonly ITenantInfo _tenantInfo;

        public GetClaimStatusTotalReportQueryHandler(IStringLocalizer<GetClaimStatusTotalReportQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService, ITenantInfo tenantInfo)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
            _tenantInfo = tenantInfo;
        }

        public async Task<Result<List<GetClaimStatusTotalReportResponse>>> Handle(GetClaimStatusTotalReportQuery query, CancellationToken cancellationToken)
        {
            List<GetClaimStatusTotalReportResponse> response = await _claimStatusQueryService.GetClaimStatusTotalReportAsync(_tenantInfo.Identifier);

            return await Result<List<GetClaimStatusTotalReportResponse>>.SuccessAsync(response);
        }
    }
}
