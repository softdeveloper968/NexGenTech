using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.CustomAttributes;
using MedHelpAuthorizations.Domain.CustomAttributes.CustomReport;
using MedHelpAuthorizations.Shared.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Reports.CustomReports.Query
{
    public class CustomPreviewsReportQuery : IRequest<Result<UpdatedClaimReportTypePreviewModel>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 30;
        public int ClientId { get; set; } = 0;
        public CustomReportTypeEnum CustomReportType { get; set; }
        public List<CustomReportSelectedColumns> ChoosedColumns { get; set; }
        public bool HasColumnsChoosed { get; set; }
        //public ClaimReportTypeColumns CustomFilterOptions { get; set; }///Remove : currently using dynamic models
        public bool HasCustomFilterOptions { get; set; }
        public bool AllowLimitPagination { get; set; } = false;
        public Dictionary<string, object> SetFilterColumnsWithValues { get; set; }
        public string CustomDateQueryForWhereClause { get; set; } = string.Empty;
        public List<string> TableNames { get; set; } = new();
    }

    public class PreviewCustomReportQueryHandler : IRequestHandler<CustomPreviewsReportQuery, Result<UpdatedClaimReportTypePreviewModel>>
    {
        private readonly ICustomReportService _customReportService;
        private readonly IStringLocalizer<PreviewCustomReportQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public PreviewCustomReportQueryHandler(ICustomReportService customReportService, IStringLocalizer<PreviewCustomReportQueryHandler> localizer, IMapper mapper, ICurrentUserService userService)
        {
            _customReportService = customReportService;
            _localizer = localizer;
            _mapper = mapper;
            _currentUserService = userService;
        }

        public async Task<Result<UpdatedClaimReportTypePreviewModel>> Handle(CustomPreviewsReportQuery request, CancellationToken cancellationToken)
        {
            if (request.ClientId == 0)
            {
                request.ClientId = _currentUserService.ClientId;
            }

            string sqlQuery = _customReportService.GenerateDynamicSQLQuery(request, out string columnsForSQLQuery, request.PageNumber, request.PageSize, allowLimitPagination: request.AllowLimitPagination);
            ///Execute SQL Query.
            if (!string.IsNullOrEmpty(sqlQuery))
            {
                UpdatedClaimReportTypePreviewModel response = await _customReportService.ExecutionClaimReportTypeSQLQuery(claimSQLquery: sqlQuery, columnsForSQLQuery, request.AllowLimitPagination);

                ///Add columnsForSQLQuery.
                response.ColumnsForSQLQuery = columnsForSQLQuery;
                ///Add SQL Report Without Pagination offset rows.
                response.PreviewReportSQLQuery = sqlQuery.Split("OFFSET").FirstOrDefault() ?? string.Empty;
                return Result<UpdatedClaimReportTypePreviewModel>.Success(response);
            }
            else
            {
                ///Todo: Handle If Query not generated.
            }
            return null;
        }

    }
}
