using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.IntegratedServices.Charges;
using MedHelpAuthorizations.Application.Helpers;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.ExportReport
{
    /// <summary>
    /// Represents a query for exporting Claim Status Date Lag data.
    /// </summary>
    public class ExportClaimStatusDateLagQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<string>
    {
    }

    /// <summary>
    /// Handles the export functionality for Claim Status Date Lag data.
    /// </summary>
    public class ExportClaimStatusDateLagQueryHandler : IRequestHandler<ExportClaimStatusDateLagQuery, string>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportClaimStatusDateLagQueryHandler> _localizer;

        // Constructor for ExportClaimStatusDateLagQueryHandler.
        // Parameters:
        //   - claimStatusQueryService: Service for executing Claim Status queries.
        //   - excelService: Service for Excel-related operations.
        //   - localizer: Localizer for handling string localization.
        public ExportClaimStatusDateLagQueryHandler(IClaimStatusQueryService claimStatusQueryService, IExcelService excelService, IStringLocalizer<ExportClaimStatusDateLagQueryHandler> localizer)
        {
            _claimStatusQueryService = claimStatusQueryService;
            _excelService = excelService;
            _localizer = localizer;
        }

        /// <summary>
        /// Handles the export request for Claim Status Date Lag data.
        /// </summary>
        /// <param name="request">The export query containing filter parameters.</param>
        /// <param name="cancellationToken">Token for canceling the operation.</param>
        /// <returns></returns>
        public async Task<string> Handle(ExportClaimStatusDateLagQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a query to retrieve the cash projection data
                var query = new ClaimStatusDateLagQuery()
                {
                    // Populate query parameters from the export request
                    ClaimStatusBatchId = request.ClaimStatusBatchId,
                    ClientLocationIds = request.ClientLocationIds,
                    ClientProviderIds = request.ClientProviderIds,
                    ClientInsuranceIds = request.ClientInsuranceIds,
                    ExceptionReasonCategoryIds = request.ExceptionReasonCategoryIds,
                    ProcedureCodes = request.ProcedureCodes,
                    PatientId = request.PatientId,
                    CommaDelimitedLineItemStatusIds = request.CommaDelimitedLineItemStatusIds,
                    ReceivedFrom = request.ReceivedFrom,
                    ReceivedTo = request.ReceivedTo,
                    DateOfServiceFrom = request.DateOfServiceFrom,
                    DateOfServiceTo = request.DateOfServiceTo,
                    TransactionDateFrom = request.TransactionDateFrom,
                    TransactionDateTo = request.TransactionDateTo,
                    ClaimBilledFrom = request.ClaimBilledFrom,
                    ClaimBilledTo = request.ClaimBilledTo,
                };

                // Query the service to execute the stored procedure
                List<ClaimStatusDateLagResponse> response = await _claimStatusQueryService.GetClaimStatusDateLagAsync(query);

                var exportResponse = response.Select(z=>new ExportQueryResponse
                 {
                     ServiceToBilledDateLag = z.ServiceToBilledDateLag,
                     ServiceToPaymentDateLag = z.ServiceToPaymentDateLag,
                     BilledToPaymentDateLag = z.BilledToPaymentDateLag
                 });
                // Get the Excel report mappings for the summary data.
                var excelReport = _claimStatusQueryService.GetExcelDateLagReport();

                // Export data to Excel using the Excel service
                var exportData = await _excelService.ExportAsync(exportResponse, workSheetName: _localizer["Export Details"], mappers: excelReport, sheetName: _localizer["Date Lag Report"]).ConfigureAwait(true);

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

