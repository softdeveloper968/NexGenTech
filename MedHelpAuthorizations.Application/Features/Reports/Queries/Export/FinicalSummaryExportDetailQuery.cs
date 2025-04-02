using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class FinicalSummaryExportDetailQuery : ClaimStatusDashboardDetailsQueryBase, IRequest<string>, IClaimStatusDashboardStandardQuery
    {

    }

    public class FinicalSummaryExportDetailQueryHandler : IRequestHandler<FinicalSummaryExportDetailQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<FinicalSummaryExportDetailQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IReportJobService _reportJobService;
        private readonly ITenantInfo _tenantInfo;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<FinicalSummaryExportDetailQuery> _logger;
        public FinicalSummaryExportDetailQueryHandler(IExcelService excelService
            , IClaimStatusQueryService claimStatusQueryService
            , IStringLocalizer<FinicalSummaryExportDetailQueryHandler> localizer
            , IMapper mapper,
            IReportJobService reportJobService, ITenantInfo tenantInfo, ICurrentUserService currentUserService, ILogger<FinicalSummaryExportDetailQuery> logger)
        {
            _excelService = excelService;
            _claimStatusQueryService = claimStatusQueryService;
            _localizer = localizer;
            _mapper = mapper;
            _reportJobService = reportJobService;
            _tenantInfo = tenantInfo;
            _currentUser = currentUserService;
            _logger = logger;
        }

        public async Task<string> Handle(FinicalSummaryExportDetailQuery request, CancellationToken cancellationToken)
        {
            string conn = _tenantInfo.ConnectionString;
            int clientId = _currentUser.ClientId;
            string tenantIdentifier = _tenantInfo.Identifier;
            try
            {
                if (!String.IsNullOrEmpty(conn))
                {
                    _logger.LogInformation("Processing FinicalSummaryExportDetailQuery for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);
                    // Enqueue the background job for report generation
                    var data = BackgroundJob.Enqueue(() => _reportJobService.GenerateFinancialSummaryReportAsync(request, _currentUser.UserId, clientId,tenantIdentifier ,conn));
                    _logger.LogInformation("Hangfire job enqueued successfully with Job ID: {JobId}, FileName: {FileName}", data, request.FileName);
                    // Return the job ID to the UI
                    return data;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while enqueuing Hangfire job for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);
                throw;
            }

        }
    }
}