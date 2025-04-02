using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetAverageDaysToPayByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<AverageDaysByProvider>>>
    {
    }

    public class GetAverageDaysToPayByProviderQueryHandler : IRequestHandler<GetAverageDaysToPayByProviderQuery, Result<List<AverageDaysByProvider>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetAverageDaysToPayByProviderQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetAverageDaysToPayByProviderQueryHandler(IStringLocalizer<GetAverageDaysToPayByProviderQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<AverageDaysByProvider>>> Handle(GetAverageDaysToPayByProviderQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetAverageDaysToPayByProviderAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<AverageDaysByProvider>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
