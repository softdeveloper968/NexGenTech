using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProviderByProcedureQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProviderTotalsByProcedure>>>
    {
    }

    public class GetProviderByProcedureQueryHandler : IRequestHandler<GetProviderByProcedureQuery, Result<List<ProviderTotalsByProcedure>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProviderByProcedureQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProviderByProcedureQueryHandler(IStringLocalizer<GetProviderByProcedureQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProviderTotalsByProcedure>>> Handle(GetProviderByProcedureQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProviderTotalsByProcedureAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProviderTotalsByProcedure>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
