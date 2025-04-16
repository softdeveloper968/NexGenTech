using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProviderReimbursementByProcedureCodeQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProviderReimbursementByProcedureCodeSummary>>>
    {
    }

    public class GetProviderReimbursementByProcedureCodeQueryHandler : IRequestHandler<GetProviderReimbursementByProcedureCodeQuery, Result<List<ProviderReimbursementByProcedureCodeSummary>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProviderReimbursementByProcedureCodeQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProviderReimbursementByProcedureCodeQueryHandler(IStringLocalizer<GetProviderReimbursementByProcedureCodeQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProviderReimbursementByProcedureCodeSummary>>> Handle(GetProviderReimbursementByProcedureCodeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetProviderReimbursementByProcedureCodeAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProviderReimbursementByProcedureCodeSummary>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

