using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ExportClaimStatusDenialsReportQuery : ClaimStatusDashboardDetailsQueryBase, IRequest<string>, IClaimStatusDashboardStandardQuery
    {

    }

    public class ExportClaimStatusDenialsReportQueryHandler : IRequestHandler<ExportClaimStatusDenialsReportQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportClaimStatusDenialsReportQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IReportJobService _reportJobService;
        private readonly ICurrentUserService _currentUser;
        private readonly ITenantInfo _tenantInfo;
        private readonly ILogger<ExportClaimStatusDenialsReportQueryHandler> _logger;

        public ExportClaimStatusDenialsReportQueryHandler(IExcelService excelService
            , IClaimStatusQueryService claimStatusQueryService
            , IStringLocalizer<ExportClaimStatusDenialsReportQueryHandler> localizer
            , IMapper mapper, IReportJobService reportJobService, ICurrentUserService currentUserService, ITenantInfo tenantInfo, ILogger<ExportClaimStatusDenialsReportQueryHandler> logger)
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

        public async Task<string> Handle(ExportClaimStatusDenialsReportQuery request, CancellationToken cancellationToken) //Updated EN-62
        {
            _logger.LogInformation("Starting Handle method for ExportClaimStatusDenialsReportQuery.");
            try
            {
                string conn = _tenantInfo.ConnectionString;
                int clientId = _currentUser.ClientId;
                string tenantIdentifier = _tenantInfo.Identifier;
                _logger.LogInformation("Enqueuing background job for ExportClaimStatusDenialsReportQuery with ClientId: {ClientId}, UserId: {UserId}", clientId, _currentUser.UserId);

                var data = BackgroundJob.Enqueue(() => _reportJobService.GetClaimStatusDenialsAsync(request, _currentUser.UserId, clientId, conn, tenantIdentifier));
                _logger.LogInformation("Successfully enqueued ExportClaimStatusDenialsReportQuery job.");
                return data;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred in Handle method for ExportClaimStatusDenialsReportQuery.");
                throw;
            }
        }
    }
}
