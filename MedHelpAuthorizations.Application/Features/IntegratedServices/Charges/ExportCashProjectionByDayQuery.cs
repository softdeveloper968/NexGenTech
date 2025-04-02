using MedHelpAuthorizations.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    public class ExportCashProjectionByDayQuery : IRequest<string>
    {
        public int FilterForDays { get; set; } = 7; //AA-343
        public string ClientLocationIds { get; set; } = string.Empty; //AA-343
        public string ClientProviderIds { get; set; } = string.Empty; //AA-343
    }

    public class ExportCashProjectionByDayQueryHandler : IRequestHandler<ExportCashProjectionByDayQuery, string>
    { 
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportCashProjectionByDayQueryHandler> _localizer;

        public ExportCashProjectionByDayQueryHandler(IClaimStatusQueryService claimStatusQueryService, IExcelService excelService, IStringLocalizer<ExportCashProjectionByDayQueryHandler> localizer)
        {
            _claimStatusQueryService = claimStatusQueryService;
            _excelService = excelService;
            _localizer = localizer;
        }

        public async Task<string> Handle(ExportCashProjectionByDayQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Create a query to retrieve the cash projection data
                GetCashProjectionByDayQuery query = new();
                query.FilterForDays = request.FilterForDays;
                query.ClientProviderIds = request.ClientProviderIds;
                query.ClientLocationIds = request.ClientLocationIds;
                // Query the service to execute the stored procedure
                var response = await _claimStatusQueryService.GetCashProjectionByDayAsync(query);

                // Get the Excel report mappings for the summary data.
                var excelReport = _claimStatusQueryService.GetExcelCashProjection();

                var exportData = await _excelService.ExportAsync(response, workSheetName: _localizer["Export Details"], mappers: excelReport, sheetName: _localizer["Cash Projection Report"]).ConfigureAwait(true);
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
