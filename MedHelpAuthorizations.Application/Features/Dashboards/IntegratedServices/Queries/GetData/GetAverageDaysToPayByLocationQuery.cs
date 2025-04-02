using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetAverageDaysToPayByLocationQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<AverageDaysByLocation>>>
    {
    }

    public class GetAverageDaysToPayByLocationQueryHandler : IRequestHandler<GetAverageDaysToPayByLocationQuery, Result<List<AverageDaysByLocation>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetAverageDaysToPayByLocationQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetAverageDaysToPayByLocationQueryHandler(IStringLocalizer<GetAverageDaysToPayByLocationQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<AverageDaysByLocation>>> Handle(GetAverageDaysToPayByLocationQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetAverageDaysToPayByLocationAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<AverageDaysByLocation>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<AverageDaysByLocation>>.FailAsync(ex.Message);

            }
        }
    }
}
