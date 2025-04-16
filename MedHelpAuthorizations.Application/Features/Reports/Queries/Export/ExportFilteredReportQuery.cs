using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ExportFilteredReportQuery : ClaimStatusDashboardQueryBase, IClaimStatusDashboardStandardQuery, IRequest<string>
    {
    }

    public class ExportFilteredReportQueryHandler : IRequestHandler<ExportFilteredReportQuery, string>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportFilteredReportQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExcelService _excelService;

        private int _clientId => _currentUserService.ClientId;

        public ExportFilteredReportQueryHandler(IStringLocalizer<ExportFilteredReportQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService, ICurrentUserService currentUserService, IExcelService excelService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
            _currentUserService = currentUserService;
            _excelService = excelService;
        }

        public async Task<string> Handle(ExportFilteredReportQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _claimStatusQueryService.GetFilteredReportAsync(query, _clientId);
                // Get the Excel report mappings for the summary data.
                var excelReport = _claimStatusQueryService.GetExcelFilteredClaimsReport();

                // Export data to Excel using the Excel service
                var exportData = await _excelService.ExportAsync(response, mappers: excelReport, sheetName: _localizer["Filtered Claims Report"], workSheetName: _localizer["Export Details"], null, true).ConfigureAwait(true);

                // Return the exported data as a string
                return exportData;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
