using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ExportAvgDayToPayReportDetailsQuery : ClaimStatusDashboardDetailsQueryBase, IRequest<string>, IClaimStatusDashboardStandardQuery
    {
        public int ClientId { get; set; } = 0;
        public string DelimitedLineItemStatusIds { get; set; } = string.Empty;
        public string ClientAuthTypeIds { get; set; } = string.Empty;
        public string ClientProcedureCodes { get; set; } = string.Empty;
        public string ClientExceptionReasonCategoryIds { get; set; } = string.Empty;
        public bool HasAvgDayToPayByProvider { get; set; } = false;
    }

    public class ExportAvgDayToPayReportDetailsQueryHandler : IRequestHandler<ExportAvgDayToPayReportDetailsQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportAvgDayToPayReportDetailsQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IReportJobService _reportJobService;
        private readonly ICurrentUserService _currentUser;
        private readonly ITenantInfo _tenantInfo;
        private readonly ILogger<ExportAvgDayToPayReportDetailsQueryHandler> _logger;
        public ExportAvgDayToPayReportDetailsQueryHandler(IExcelService excelService, IClaimStatusQueryService claimStatusQueryService, IStringLocalizer<ExportAvgDayToPayReportDetailsQueryHandler> localizer, IMapper mapper, IReportJobService reportJobService, ICurrentUserService currentUserService, ITenantInfo tenantInfo, ILogger<ExportAvgDayToPayReportDetailsQueryHandler> logger)
        {
            _excelService = excelService;
            _claimStatusQueryService = claimStatusQueryService;
            _localizer = localizer;
            _mapper = mapper;
            _reportJobService = reportJobService;
            _currentUser = currentUserService;
            _tenantInfo = tenantInfo;
            _logger = logger;
        }

        public async Task<string> Handle(ExportAvgDayToPayReportDetailsQuery request, CancellationToken cancellationToken) //Updated EN-62
        {
            string conn = _tenantInfo.ConnectionString;
            int clientId = _currentUser.ClientId;
            string tenantIdentifier = _tenantInfo.Identifier;
            try
            {
                _logger.LogInformation("Processing ExportAvgDayToPayReportDetailsQuery for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);
                var data = BackgroundJob.Enqueue(() => _reportJobService.GetAvgDayToPayReportSummaryReportAsync(request, _currentUser.UserId, clientId, conn,tenantIdentifier));
                _logger.LogInformation("Hangfire job enqueued successfully with Job ID: {JobId}, FileName: {FileName}", data, request.FileName);
                return data;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while enqueuing Hangfire job for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);
                throw;
            }
        }
        

    }
}
