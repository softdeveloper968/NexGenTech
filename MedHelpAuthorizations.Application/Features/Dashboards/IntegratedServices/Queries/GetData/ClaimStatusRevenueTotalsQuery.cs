using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport;
using System.Linq;
using MedHelpAuthorizations.Application.Helpers;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData
{
    // This class represents a query for retrieving claim status revenue totals.
    // It is used in the context of a claim status dashboard.
    public class ClaimStatusRevenueTotalsQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<Result<List<ClaimStatusRevenueTotal>>> //AA-330
    {
    }

    // This class handles the execution of the ClaimStatusRevenueTotalsQuery and returns the result.
    public class ClaimStatusRevenueTotalsQueryHandler : IRequestHandler<ClaimStatusRevenueTotalsQuery, Result<List<ClaimStatusRevenueTotal>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ClaimStatusRevenueTotalsQueryHandler> _localizer;

        // Constructor for the query handler.
        public ClaimStatusRevenueTotalsQueryHandler(IStringLocalizer<ClaimStatusRevenueTotalsQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
        }

        // Handles the execution of the query.
        public async Task<Result<List<ClaimStatusRevenueTotal>>> Handle(ClaimStatusRevenueTotalsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                List<ExportQueryResponse> claimStatusRevenueTotals = await _claimStatusQueryService.GetClaimStatusRevenueTotalsAsync(query);
                if (claimStatusRevenueTotals.Any())
                {
                    var result = claimStatusRevenueTotals.Select(response => new ClaimStatusRevenueTotal
                    {
                        ClientInsuranceName = response.ClientInsuranceName,
                        ClaimLineItemStatus = response.ClaimLineItemStatus,
                        ClaimLineItemStatusId = (int?)response.ClaimLineItemStatusId,
                        ClaimStatusExceptionReasonCategory = response.ClaimStatusExceptionReasonCategory,
                        ProcedureCode = response.ProcedureCode,
                        Quantity = response.Quantity,
                        ChargedSum = response.ChargedSum,
                        PaidAmountSum = response.PaidAmountSum,
                        AllowedAmountSum = response.AllowedAmountSum,
                        NonAllowedAmountSum = response.NonAllowedAmountSum,
                        EntryHash = response.EntryHash,
                        WriteOffAmountSum = response.WriteOffAmountSum,
                        ClaimBilledOn = response.ClaimBilledOn.HasValue ? response.ClaimBilledOn.Value.ToString(ClaimFiltersHelpers._dateFormat) : string.Empty,
                        DateOfServiceFrom = response.DateOfServiceFrom.HasValue ? response.DateOfServiceFrom.Value.ToString(ClaimFiltersHelpers._dateFormat) : string.Empty
                    }).ToList();

                    // Return a successful result with the response data.
                    return await Result<List<ClaimStatusRevenueTotal>>.SuccessAsync(result);
                }
                return await Result<List<ClaimStatusRevenueTotal>>.FailAsync($"Data not found,while getting Claim Status Revenue Totals.");
            }
            catch (Exception ex)
            {
                return await Result<List<ClaimStatusRevenueTotal>>.FailAsync($"Error while getting Claim Status Revenue Totals. {ex.Message}");
            }
        }
    }
}
