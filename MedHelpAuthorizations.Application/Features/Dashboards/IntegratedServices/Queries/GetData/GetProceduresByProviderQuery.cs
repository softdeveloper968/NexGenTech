using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProceduresByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProcedureTotalsByProvider>>>
    {
    }
    public class GetProceduresByProviderQueryHandler : IRequestHandler<GetProceduresByProviderQuery, Result<List<ProcedureTotalsByProvider>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProceduresByProviderQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProceduresByProviderQueryHandler(IStringLocalizer<GetProceduresByProviderQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProcedureTotalsByProvider>>> Handle(GetProceduresByProviderQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProcedureTotalsByProviderAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProcedureTotalsByProvider>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
