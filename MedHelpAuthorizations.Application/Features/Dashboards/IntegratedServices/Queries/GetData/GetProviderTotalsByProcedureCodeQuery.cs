using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProviderTotalsByProcedureCodeQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProviderTotals>>>
    {

    }

    public class GetProviderTotalsByProcedureCodeQueryHandler : IRequestHandler<GetProviderTotalsByProcedureCodeQuery, Result<List<ProviderTotals>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProviderTotalsByProcedureCodeQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProviderTotalsByProcedureCodeQueryHandler(IStringLocalizer<GetProviderTotalsByProcedureCodeQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProviderTotals>>> Handle(GetProviderTotalsByProcedureCodeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProviderTotalsByProcedureCodeAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProviderTotals>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
