using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.CustomAttributes.CustomReport;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Helpers;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{
    public class ExportPreviewReportQuery : IRequest<Result<string>>
    {
        public string PreviewReportSQLQuery { get; set; }
        public string ColumnsForSQLQuery { get; set; } = string.Empty;
        public string CustomReportTitle { get; set; } = string.Empty;
        public CustomReportFileTypeEnum CustomReportFileTypeEnum { get; set; }
    }

    public class ExportPreviewReportQueryHandler : IRequestHandler<ExportPreviewReportQuery, Result<string>>
    {
        private readonly ICustomReportService _customReportService;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<ExportPreviewReportQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly string errorMessage = $"Error while exporting CustomReport.";

        public ExportPreviewReportQueryHandler(ICustomReportService customReportService, IStringLocalizer<ExportPreviewReportQueryHandler> localizer, IMapper mapper, ICurrentUserService userService, IExcelService excelService)
        {
            _customReportService = customReportService;
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = userService;
            _excelService = excelService;
        }

        public async Task<Result<string>> Handle(ExportPreviewReportQuery request, CancellationToken cancellationToken)
        {
            try
            {
                List<string> exportReportDetails = new();
                List<string> exportCustomReportSheets = new();
                List<List<string>> exportReportHeaderDetails = new();

                if (string.IsNullOrEmpty(request.PreviewReportSQLQuery))
                {
                   return await Result<string>.FailAsync("Preview Report Query not found.").ConfigureAwait(false);
                }
                if (string.IsNullOrEmpty(request.ColumnsForSQLQuery))
                {
                   return await Result<string>.FailAsync("Selected Columns Not Found").ConfigureAwait(false);
                }

                switch (request.CustomReportFileTypeEnum)
                {
                    case CustomReportFileTypeEnum.xlsx:
                        {

                            ///Get Preview Report data.
                            string response =await _customReportService.ExecutionPreviewClaimReportTypeSQLQuery(claimSQLquery: request.PreviewReportSQLQuery, includeColumns: false);

                            if (!string.IsNullOrEmpty(response))
                            {
                                exportReportDetails.Add(response);
                            }
                            if (!string.IsNullOrEmpty(request.ColumnsForSQLQuery))
                            {
                                List<string> headers = CustomReportHelper.GetExtractedColumns(request.ColumnsForSQLQuery);
                                if (headers.Any())
                                {
                                    exportReportHeaderDetails.Add(headers);
                                }
                            }
                            if (!string.IsNullOrEmpty(request.CustomReportTitle))
                            {
                                exportCustomReportSheets.Add(request.CustomReportTitle);
                            }

                            string result = await _excelService.ExportMultipleCustomReportTabsInWorksheet(exportReportDetails, exportReportHeaderDetails, exportCustomReportSheets);
                            return await Result<string>.SuccessAsync(data: result);
                        }
                    case CustomReportFileTypeEnum.csv:
                        {
                            ///Get Preview Report data.
                            string response =await _customReportService.ExecutionPreviewClaimReportTypeSQLQuery(claimSQLquery: request.PreviewReportSQLQuery, includeColumns: true);

                            return await Result<string>.SuccessAsync(data: response);
                        }
                }
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
