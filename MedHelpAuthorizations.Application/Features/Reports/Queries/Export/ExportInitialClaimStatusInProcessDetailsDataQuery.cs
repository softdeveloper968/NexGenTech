using AutoMapper;
using Finbuckle.MultiTenant;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export.ClaimStatus
{
    public class ExportInitialClaimStatusInProcessDetailsQuery : ClaimStatusDashboardQueryBase, IRequest<string>, IInitialClaimStatusDashboardDetailsQuery
    {
        public string FlattenedLineItemStatus { get; set; }
    }

    public class ExportInitialClaimStatusInProcessDetailsQueryHandler : IRequestHandler<ExportInitialClaimStatusInProcessDetailsQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportInitialClaimStatusInProcessDetailsQueryHandler> _localizer;
        private readonly IMapper _mapper;
        private readonly IReportJobService _reportJobService;
        private readonly ITenantInfo _tenantInfo;
        private readonly ICurrentUserService _currentUser;
        private readonly ILogger<ExportInitialClaimStatusInProcessDetailsQueryHandler> _logger;

        public ExportInitialClaimStatusInProcessDetailsQueryHandler(IExcelService excelService
            , IClaimStatusQueryService claimStatusQueryService
            , IStringLocalizer<ExportInitialClaimStatusInProcessDetailsQueryHandler> localizer
            , IMapper mapper, IReportJobService reportJobService, ITenantInfo tenantInfo, ICurrentUserService currentUserService, ILogger<ExportInitialClaimStatusInProcessDetailsQueryHandler> logger)
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

        public async Task<string> Handle(ExportInitialClaimStatusInProcessDetailsQuery request, CancellationToken cancellationToken)
        {
            string conn = _tenantInfo.ConnectionString;
            int clientId = _currentUser.ClientId;
            string tenantIdentifier = _tenantInfo.Identifier;
            try
            {
                _logger.LogInformation("Processing ExportClaimStatusDenialDetailsQuery for User: {UserId}, Client: {ClientId}, FileName: {FileName}", _currentUser.UserId, clientId, request.FileName);

                var data = await _reportJobService.ExportInitialClaimStatusInProcessDetails(request, _currentUser.UserId, clientId, conn, tenantIdentifier);
                _logger.LogInformation("Hangfire job enqueued successfully with Job ID: {JobId}, FileName: {FileName}", data, request.FileName);
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