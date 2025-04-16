using AutoMapper;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData
{
    public class ClaimStatusDashboardDenialsDetailQuery : ClaimStatusDashboardQuery, IRequest<Result<List<ClaimStatusDashboardDetailsResponse>>>, IClaimStatusDashboardDetailsQuery
    {
        public string FlattenedLineItemStatus { get; set; } = string.Empty;

        public ClaimStatusDashboardDenialsDetailQuery()
        {
        }

    }

    public class ClaimStatusDashboardDenialsDetailsQueryHandler : IRequestHandler<ClaimStatusDashboardDenialsDetailQuery, Result<List<ClaimStatusDashboardDetailsResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusDashboardDenialsDetailsQueryHandler> _localizer;
        private readonly IMapper _mapper;

        public ClaimStatusDashboardDenialsDetailsQueryHandler(IStringLocalizer<ClaimStatusDashboardDenialsDetailsQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService, IMapper mapper)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
            _mapper = mapper;
        }

        public async Task<Result<List<ClaimStatusDashboardDetailsResponse>>> Handle(ClaimStatusDashboardDenialsDetailQuery query, CancellationToken cancellationToken)
        {
            var dashboardDetails = await _claimStatusQueryService.GetDenialDetailsAsync(query);
            var response = _mapper.Map<List<ClaimStatusDashboardDetailsResponse>>(dashboardDetails);

            return await Result<List<ClaimStatusDashboardDetailsResponse>>.SuccessAsync(response);
        }
    }
}
