using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetAverageDaysToPayByPayerQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<AverageDaysByPayer>>> //AA-330
    {
    }

    public class GetAverageDaysToPayByPayerQueryHandler : IRequestHandler<GetAverageDaysToPayByPayerQuery, Result<List<AverageDaysByPayer>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetAverageDaysToPayByPayerQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetAverageDaysToPayByPayerQueryHandler(IStringLocalizer<GetAverageDaysToPayByPayerQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<AverageDaysByPayer>>> Handle(GetAverageDaysToPayByPayerQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetAverageDaysToPayByPayerAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<AverageDaysByPayer>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
