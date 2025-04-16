using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus
{
    public class ExportClaimStatusDetailsQuery : ClaimStatusDashboardDetailsQueryBase, IRequest<string>, IClaimStatusDashboardStandardQuery
    {

    }

    public class ExportClaimStatusDetailsQueryHandler : IRequestHandler<ExportClaimStatusDetailsQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportClaimStatusDetailsQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IReportJobService _reportJobService;
        private readonly ICurrentUserService _currentUser;
        private readonly ITenantInfo _tenantInfo;
        private readonly ILogger<ExportClaimStatusDetailsQueryHandler> _logger;

        public ExportClaimStatusDetailsQueryHandler(IExcelService excelService
            , IClaimStatusQueryService claimStatusQueryService
            , IStringLocalizer<ExportClaimStatusDetailsQueryHandler> localizer
            , IMapper mapper, IReportJobService reportJobService, ICurrentUserService currentUserService, ITenantInfo tenantInfo, ILogger<ExportClaimStatusDetailsQueryHandler> logger)
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

        public async Task<string> Handle(ExportClaimStatusDetailsQuery request, CancellationToken cancellationToken) //Updated EN-62
        {
            string conn = _tenantInfo.ConnectionString;
            int clientId = _currentUser.ClientId;
            string tenantIdentifier = _tenantInfo.Identifier;
            _logger.LogInformation("Processing ExportClaimStatusDetailsQuery for User: {UserId}, Client: {ClientId}", _currentUser.UserId, clientId);

            try
            {
                // Enqueue the background job for report generation
                _logger.LogInformation("Enqueuing Hangfire job for GetClaimStatusDetailsSummaryReportAsync for User: {UserId}, Client: {ClientId}",
                    _currentUser.UserId, clientId);

                var jobId = BackgroundJob.Enqueue(() => _reportJobService.GetClaimStatusDetailsSummaryReportAsync(request, _currentUser.UserId, clientId, conn, tenantIdentifier));

                _logger.LogInformation("Hangfire job enqueued successfully with Job ID: {JobId}", jobId);

                return jobId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while enqueuing Hangfire job for User: {UserId}, Client: {ClientId}", _currentUser.UserId, clientId);
                throw;
            }
        }

    }
}