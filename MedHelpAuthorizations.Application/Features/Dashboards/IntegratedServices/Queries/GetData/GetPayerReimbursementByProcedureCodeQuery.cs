using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    public class GetPayerReimbursementByProcedureCodeQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<PayerReimbursementByProcedureCode>>>
    {
    }
    public class GetPayerReimbursementByProcedureCodeQueryHandler : IRequestHandler<GetPayerReimbursementByProcedureCodeQuery, Result<List<PayerReimbursementByProcedureCode>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<GetPayerReimbursementByProcedureCodeQueryHandler> _localizer;

        // Constructor for the query handler.
        public GetPayerReimbursementByProcedureCodeQueryHandler(IStringLocalizer<GetPayerReimbursementByProcedureCodeQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        public async Task<Result<List<PayerReimbursementByProcedureCode>>> Handle(GetPayerReimbursementByProcedureCodeQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                var response = await _claimStatusQueryService.GetPayerReimbursementByProcedureCodeAsync(query) ?? new();

                // Return a successful result with the response data.
                return await Result<List<PayerReimbursementByProcedureCode>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
