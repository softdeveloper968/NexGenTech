using AutoMapper;
using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ExportInitialClaimStatusDenialsQuery : ClaimStatusDashboardQueryBase, IInitialClaimStatusDashboardDetailsQuery, IRequest<string>
    {
    }

    public class ExportInitialClaimStatusDenialsQueryHandler : IRequestHandler<ExportInitialClaimStatusDenialsQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportInitialClaimStatusDenialsQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IReportJobService _reportJobService;
        private readonly ICurrentUserService _currentUser;
        private readonly ITenantInfo _tenantInfo;
        private readonly ILogger<ExportInitialClaimStatusDenialsQuery> _logger;

        public ExportInitialClaimStatusDenialsQueryHandler(IExcelService excelService
            , IClaimStatusQueryService claimStatusQueryService
            , IStringLocalizer<ExportInitialClaimStatusDenialsQueryHandler> localizer
            , IMapper mapper, IReportJobService reportJobService, ICurrentUserService currentUserService, ITenantInfo tenantInfo, ILogger<ExportInitialClaimStatusDenialsQuery> logger)
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

        public async Task<string> Handle(ExportInitialClaimStatusDenialsQuery request, CancellationToken cancellationToken)
        {
            string conn = _tenantInfo.ConnectionString;
            int clientId = _currentUser.ClientId;
            string tenantString = _tenantInfo.Identifier;
            try
            {
                _logger.LogInformation("Processing ExportInitialClaimStatusDenialsQuery for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);
                var data = BackgroundJob.Enqueue(() => _reportJobService.GetInitialClaimStatusDenialsReportAsync(request, _currentUser.UserId, clientId, conn, tenantString));
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
