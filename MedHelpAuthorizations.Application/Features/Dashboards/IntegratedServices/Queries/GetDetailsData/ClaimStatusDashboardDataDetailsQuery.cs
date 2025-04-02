using AutoMapper;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetDetailsData
{
    public class ClaimStatusDashboardDetailsQuery : ClaimStatusDashboardDetailsQueryBase, IRequest<Result<List<ClaimStatusDashboardDetailsResponse>>>
    {
    }

    public class ClaimStatusDashboardDetailsQueryHandler : IRequestHandler<ClaimStatusDashboardDetailsQuery, Result<List<ClaimStatusDashboardDetailsResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusDashboardDetailsQueryHandler> _localizer;
        private readonly IMapper _mapper;

        public ClaimStatusDashboardDetailsQueryHandler(IStringLocalizer<ClaimStatusDashboardDetailsQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService, IMapper mapper)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
            _mapper = mapper;
        }

        public async Task<Result<List<ClaimStatusDashboardDetailsResponse>>> Handle(ClaimStatusDashboardDetailsQuery query, CancellationToken cancellationToken)
        {
            var dashboardDetails = await _claimStatusQueryService.GetClaimsStatusDetailsAsync(query);  //ToDo-AA-56

            var response = _mapper.Map<List<ClaimStatusDashboardDetailsResponse>>(dashboardDetails);
            return await Result<List<ClaimStatusDashboardDetailsResponse>>.SuccessAsync(response);
        }
    }
}