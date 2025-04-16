using Finbuckle.MultiTenant;
using Hangfire;
using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.Charges
{
    public class ExportCashValueForRevenueQuery : IRequest<string>
    {
        /// <summary>
        /// Date of the selected row
        /// </summary>
        public string SelectedDate { get; set; }

        /// <summary>
        /// If value is "ServiceDate" then the items will be filtered based on their service date
        /// If value is "BilledDate" then the items will be filtered based on their billed date 
        /// </summary>
        public string FilterBy { get; set; }
        public int FilterForDays { get; set; } = -7; //AA-331
        public string ClientLocationIds { get; set; } = string.Empty; //AA-331
        public string ClientProviderIds { get; set; } = string.Empty; //AA-331
        public int ClientInsuranceId { get; set; } = 0;
        public string FileName { get; set; } = string.Empty;
    }

    public class ExportCashValueForRevenueQueryHandler : IRequestHandler<ExportCashValueForRevenueQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportCashValueForRevenueQueryHandler> _localizer;
        private readonly IReportJobService _reportJobService;
        private readonly ITenantInfo _tenantInfo;
        private readonly ICurrentUserService _currentUser;

        public ExportCashValueForRevenueQueryHandler(IExcelService excelService
            , IClaimStatusQueryService claimStatusQueryService
            , IStringLocalizer<ExportCashValueForRevenueQueryHandler> localizer,
            IReportJobService reportJobService, ITenantInfo tenantInfo, ICurrentUserService currentUserService)
        {
            _excelService = excelService;
            _claimStatusQueryService = claimStatusQueryService;
            _localizer = localizer;
            _reportJobService = reportJobService;
            _tenantInfo = tenantInfo;
            _currentUser = currentUserService;
        }

        /// <summary>
        /// Handles the ExportCashValueForRevenueQuery and generates an Excel export of cash value for revenue data.
        /// </summary>
        /// <param name="request">The query specifying the export parameters.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>A base64-encoded string representing the Excel export data.</returns>
        public async Task<string> Handle(ExportCashValueForRevenueQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string conn = _tenantInfo.ConnectionString;
                int clientId = _currentUser.ClientId;
                string tenantIdentifier = _tenantInfo.Identifier;

                // Enqueue the background job for report generation
                var data = BackgroundJob.Enqueue(() => _reportJobService.ExportCashValueForRevenueAsync(request, _currentUser.UserId, clientId, conn, tenantIdentifier));

                // Return the job ID to the UI
                return data;

            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, or rethrow if necessary
                throw;
            }
        }
        


    }

}
