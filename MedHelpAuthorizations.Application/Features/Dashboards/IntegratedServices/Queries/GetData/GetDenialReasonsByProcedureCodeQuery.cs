using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetDenialReasonsByProcedureCodeQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<DenialsByProcedureSummary>>>
    {
    }

    public class GetDenialReasonsByProcedureCodeQueryHandler : IRequestHandler<GetDenialReasonsByProcedureCodeQuery, Result<List<DenialsByProcedureSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetDenialReasonsByProcedureCodeQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetDenialReasonsByProcedureCodeQueryHandler(IStringLocalizer<GetDenialReasonsByProcedureCodeQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<DenialsByProcedureSummary>>> Handle(GetDenialReasonsByProcedureCodeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetDenialReasonsByProcedureCodeAsync(query) ?? new();

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
