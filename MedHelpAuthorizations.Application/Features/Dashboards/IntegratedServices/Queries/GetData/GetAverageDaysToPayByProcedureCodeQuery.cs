using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetAverageDaysToPayByProcedureCodeQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<AverageDaysByProcedureCode>>>
    {
    }

    public class GetAverageDaysToPayByProcedureCodeQueryHandler : IRequestHandler<GetAverageDaysToPayByProcedureCodeQuery, Result<List<AverageDaysByProcedureCode>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetAverageDaysToPayByProcedureCodeQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetAverageDaysToPayByProcedureCodeQueryHandler(IStringLocalizer<GetAverageDaysToPayByProcedureCodeQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<AverageDaysByProcedureCode>>> Handle(GetAverageDaysToPayByProcedureCodeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetAverageDaysToPayByProcedureCodeAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<AverageDaysByProcedureCode>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<AverageDaysByProcedureCode>>.FailAsync(ex.Message);

            }
        }
    }
}
