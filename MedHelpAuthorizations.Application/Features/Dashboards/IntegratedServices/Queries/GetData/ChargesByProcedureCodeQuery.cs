using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class ChargesByProcedureCodeQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ChargesTotalsByProcedureCode>>>
    {
    }

    public class ChargesByProcedureCodeQueryHandler : IRequestHandler<ChargesByProcedureCodeQuery, Result<List<ChargesTotalsByProcedureCode>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ChargesByProcedureCodeQueryHandler> _localizer;

        // Constructor for the query handler.
        public ChargesByProcedureCodeQueryHandler(IStringLocalizer<ChargesByProcedureCodeQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<ChargesTotalsByProcedureCode>>> Handle(ChargesByProcedureCodeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetChargesByProcedureCodeAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<ChargesTotalsByProcedureCode>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
