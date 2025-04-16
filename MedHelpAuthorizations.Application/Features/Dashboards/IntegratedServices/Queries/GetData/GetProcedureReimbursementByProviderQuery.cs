using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProcedureReimbursementByProviderQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProcedureReimbursementByProvider>>>
    {
    }
    public class GetProcedureReimbursementByProviderQueryHandler : IRequestHandler<GetProcedureReimbursementByProviderQuery, Result<List<ProcedureReimbursementByProvider>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProcedureReimbursementByProviderQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProcedureReimbursementByProviderQueryHandler(IStringLocalizer<GetProcedureReimbursementByProviderQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProcedureReimbursementByProvider>>> Handle(GetProcedureReimbursementByProviderQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProcedureReimbursementByProviderAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProcedureReimbursementByProvider>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
