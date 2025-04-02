using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport
{
    public class ExportRevenueAnalysisReportQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<string>
    {
    }

    /// <summary>
    /// Handles the export functionality for Revenue Totals data.
    /// </summary>
    public class ExportRevenueAnalysisReportQueryHandler : IRequestHandler<ExportRevenueAnalysisReportQuery, string>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportRevenueAnalysisReportQueryHandler> _localizer;

        // Constructor for ExportClaimStatusDateLagQueryHandler.
        // Parameters:
        //   - claimStatusQueryService: Service for executing Claim Status queries.
        //   - excelService: Service for Excel-related operations.
        //   - localizer: Localizer for handling string localization.
        public ExportRevenueAnalysisReportQueryHandler(IClaimStatusQueryService claimStatusQueryService, IExcelService excelService, IStringLocalizer<ExportRevenueAnalysisReportQueryHandler> localizer)
        {
            _claimStatusQueryService = claimStatusQueryService;
            _excelService = excelService;
            _localizer = localizer;
        }

        /// <summary>
        /// Handles the export request for Revenue Totals data.
        /// </summary>
        /// <param name="request">The export query containing filter parameters.</param>
        /// <param name="cancellationToken">Token for canceling the operation.</param>
        /// <returns></returns>
        public async Task<string> Handle(ExportRevenueAnalysisReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //create a query for the data request based on the request from the front end
                var query = new ClaimStatusRevenueTotalsQuery()
                {
                    ReceivedFrom = request.ReceivedFrom,
                    ReceivedTo = request.ReceivedTo,
                    DateOfServiceFrom = request.DateOfServiceFrom,
                    DateOfServiceTo = request.DateOfServiceTo,
                    TransactionDateFrom = request.TransactionDateFrom,
                    TransactionDateTo = request.TransactionDateTo,
                    ClaimBilledFrom = request.ClaimBilledFrom,
                    ClaimBilledTo = request.ClaimBilledTo,
                    ClientInsuranceIds = request.ClientInsuranceIds,
                    AuthTypeIds = request.AuthTypeIds,
                    ClaimStatusBatchId = request.ClaimStatusBatchId,
                    ExceptionReasonCategoryIds = request.ExceptionReasonCategoryIds,
                    ProcedureCodes = request.ProcedureCodes,
                    ClientLocationIds = request.ClientLocationIds,
                    ClientProviderIds = request.ClientProviderIds,
                    PatientId = request.PatientId
                };
                // Retrieve claim status revenue totals using the claimStatusQueryService.
                List<ExportQueryResponse> response = await _claimStatusQueryService.GetClaimStatusRevenueTotalsAsync(query);

                // Get the Excel report mappings for the summary data.
                var excelReport = _claimStatusQueryService.GetExcelRevenueTotalsReport();

                // Export data to Excel using the Excel service
                var exportData = await _excelService.ExportAsync(response, workSheetName: _localizer["Export Details"], mappers: excelReport, sheetName: _localizer["Revenue Analysis Report"]).ConfigureAwait(true);

                // Return the exported data as a string
                return exportData;
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or rethrow if necessary
                throw;
            }
        }
    }
}
