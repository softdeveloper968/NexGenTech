using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProviderProcedureTotalQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProviderProcedureTotal>>>
    {
    }
    public class GetProviderProcedureTotalQueryHandler : IRequestHandler<GetProviderProcedureTotalQuery, Result<List<ProviderProcedureTotal>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProviderProcedureTotalQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProviderProcedureTotalQueryHandler(IStringLocalizer<GetProviderProcedureTotalQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProviderProcedureTotal>>> Handle(GetProviderProcedureTotalQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProviderProcedureTotalAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProviderProcedureTotal>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
