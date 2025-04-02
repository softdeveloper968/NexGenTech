using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{
    public class ExportCustomReportQuery : IRequest<Result<string>>
    {
        public string PreviewReportSQLQuery { get; set; }

    }

    public class ExportCustomReportQueryHandler : IRequestHandler<ExportCustomReportQuery, Result<string>>
    {
        private readonly ICustomReportService _customReportService;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportCustomReportQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly string errorMessage = $"Error while exporting CustomReport.";

        public ExportCustomReportQueryHandler(ICustomReportService customReportService, IStringLocalizer<ExportCustomReportQueryHandler> localizer, IMapper mapper, ICurrentUserService userService, IExcelService excelService)
        {
            _customReportService = customReportService;
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = userService;
            _excelService = excelService;
        }

        public async Task<Result<string>> Handle(ExportCustomReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<string> exportReportDetails = new();
                List<string> exportCustomReportSheets = new();
                List<List<string>> exportReportHeaderDetails = new();

                string result = await _excelService.ExportMultipleCustomReportTabsInWorksheet(exportReportDetails,exportReportHeaderDetails,exportCustomReportSheets);
                return await Result<string>.SuccessAsync(data: result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await Result<string>.FailAsync(ex.Message).ConfigureAwait(false);
            }
            return await Result<string>.FailAsync(errorMessage).ConfigureAwait(false);
        }
    }
}
