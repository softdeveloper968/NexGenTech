using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProcedureTotalsByLocationQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProcedureTotalsbyLocationsSummary>>>
    {

        public class GetProcedureTotalsByLocationQueryHandler : IRequestHandler<GetProcedureTotalsByLocationQuery, Result<List<ProcedureTotalsbyLocationsSummary>>>
        {
            private readonly IClaimStatusQueryService _claimStatusQueryService;
            private readonly IStringLocalizer<GetProcedureTotalsByLocationQueryHandler> _localizer;

            // Constructor for the query handler.
            public GetProcedureTotalsByLocationQueryHandler(IStringLocalizer<GetProcedureTotalsByLocationQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
            {
                _localizer = localizer;
                _claimStatusQueryService = claimStatusQueryService;
            }

            public async Task<Result<List<ProcedureTotalsbyLocationsSummary>>> Handle(GetProcedureTotalsByLocationQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    // Retrieve claim status revenue totals using the claimStatusQueryService.
                    var response = await _claimStatusQueryService.GetProcedureTotalsByLocationsAsync(query) ?? new List<ProcedureTotalsbyLocationsSummary>();

                    // Return a successful result with the response data.
                    return await Result<List<ProcedureTotalsbyLocationsSummary>>.SuccessAsync(response);
                }
                catch (Exception ex)
                {
                    return await Result<List<ProcedureTotalsbyLocationsSummary>>.FailAsync(ex.Message); 
                }
            }
        }
    }
}
