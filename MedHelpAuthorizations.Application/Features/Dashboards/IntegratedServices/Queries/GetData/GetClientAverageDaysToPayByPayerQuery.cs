using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetClientAverageDaysToPayByPayerQuery : ICorporateDashboardQueryBase, IRequest<Result<List<AverageDaysByPayer>>> //AA-330
    {
        public string TenantClientString { get; set; }
    }

    public class GetClientAverageDaysToPayByPayerQueryHandler : IRequestHandler<GetClientAverageDaysToPayByPayerQuery, Result<List<AverageDaysByPayer>>>
    {
        private readonly ICorporateDashboardService _corporateDashboardService;
        private readonly IStringLocalizer<GetClientAverageDaysToPayByPayerQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetClientAverageDaysToPayByPayerQueryHandler(IStringLocalizer<GetClientAverageDaysToPayByPayerQueryHandler> localizer, ICorporateDashboardService claimStatusQueryService)
        {
            _localizer = localizer;
            _corporateDashboardService = claimStatusQueryService;
        }

        public async Task<Result<List<AverageDaysByPayer>>> Handle(GetClientAverageDaysToPayByPayerQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _corporateDashboardService.GetClientAverageDasyByPayerDataAsync(query) ?? new();

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
