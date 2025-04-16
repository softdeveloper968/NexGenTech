using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetProcedureReimbursementByLocationQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ProcedureReimbursementByLocation>>>
    {
    }
    public class GetProcedureReimbursementByLocationQueryHandler : IRequestHandler<GetProcedureReimbursementByLocationQuery, Result<List<ProcedureReimbursementByLocation>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetProcedureReimbursementByLocationQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetProcedureReimbursementByLocationQueryHandler(IStringLocalizer<GetProcedureReimbursementByLocationQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ProcedureReimbursementByLocation>>> Handle(GetProcedureReimbursementByLocationQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _claimStatusQueryService.GetProcedureReimbursementByLocationAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ProcedureReimbursementByLocation>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<ProcedureReimbursementByLocation>>.FailAsync(ex.Message);
            }
        }
    }
}
