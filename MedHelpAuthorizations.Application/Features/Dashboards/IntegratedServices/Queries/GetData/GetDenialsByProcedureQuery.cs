using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialsByProcedureQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<DenialsByProcedureSummary>>>
    {
    }
    public class GetDenialsByProcedureQueryHandler : IRequestHandler<GetDenialsByProcedureQuery, Result<List<DenialsByProcedureSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetDenialsByProcedureQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetDenialsByProcedureQueryHandler(IStringLocalizer<GetDenialsByProcedureQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<DenialsByProcedureSummary>>> Handle(GetDenialsByProcedureQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetDenialByProcedureAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<DenialsByProcedureSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
