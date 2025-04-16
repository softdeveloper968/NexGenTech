using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Client.Shared.Models.DashboardPresets;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ARAgingReportExportSummaryQuery : IRequest<string>
    {
        public DashboardPresets.PresetFilterTypeEnum PresetFilterType { get; set; } = DashboardPresets.PresetFilterTypeEnum.BilledOnDate;
        public ARAgingReportDayGroupEnum ARAgingReportDayGroupEnum { get; set; }
        public string ClientInsuranceIds { get; set; } = string.Empty;
        public string ClientLocationIds { get; set; } = string.Empty;
        public string ClientProviderIds { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public ARAgingReportExportSummaryQuery() { }
    }
    public class ARAgingReportExportSummaryQueryHandler : IRequestHandler<ARAgingReportExportSummaryQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IARAgingReportQueryService _reportQueryService;
        private readonly IStringLocalizer<ARAgingReportExportSummaryQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IReportJobService _reportJobService;
        private readonly ITenantInfo _tenantInfo;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<ARAgingReportExportSummaryQueryHandler> _logger;

        public ARAgingReportExportSummaryQueryHandler(IExcelService excelService, IARAgingReportQueryService queryService, IStringLocalizer<ARAgingReportExportSummaryQueryHandler> localizer, IMapper mapper, IMediator mediator, IReportJobService reportJobService, ITenantInfo tenantInfo, ICurrentUserService currentUserService, ILogger<ARAgingReportExportSummaryQueryHandler> logger)
        {
            _excelService = excelService;
            _reportQueryService = queryService;
            _localizer = localizer;
            _mapper = mapper;
            _mediator = mediator;
            _reportJobService = reportJobService;
            _tenantInfo = tenantInfo;
            _currentUser = currentUserService;
            _logger = logger;
        }

        public async Task<string> Handle(ARAgingReportExportSummaryQuery request, CancellationToken cancellationToken)
        {
            string conn = _tenantInfo.ConnectionString;
            int clientId = _currentUser.ClientId;
            string tenantIdentifier = _tenantInfo.Identifier;
            try
            {
                _logger.LogInformation("Processing ExportClaimStatusDenialDetailsQuery for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);
                // Enqueue the background job for report generation
                var data = BackgroundJob.Enqueue(() => _reportJobService.ARAgingReportExportSummaryAsync(request, _currentUser.UserId, clientId, conn,tenantIdentifier));
                _logger.LogInformation("Hangfire job enqueued successfully with Job ID: {JobId}, FileName: {FileName}", data, request.FileName);
                // Return the job ID to the UI
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while enqueuing Hangfire job for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);
                throw ex;
            }

        }
    }
}
