using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Models.Exports;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Reports.Queries.Export
{
    public class ExportDynamicClaimDashboardQuery : IRequest<Result<List<DynamicExportQueryResponse>>>
    {
        public int ClientId { get; set; } = 0;
        public string DelimitedLineItemStatusIds { get; set; } = string.Empty;
        public string ClientInsuranceIds { get; set; } = string.Empty;
        public string ClientAuthTypeIds { get; set; } = string.Empty;
        public string ClientProcedureCodes { get; set; } = string.Empty;
        public string ClientExceptionReasonCategoryIds { get; set; } = string.Empty;
        public string ClientProviderIds { get; set; } = string.Empty;
        public string ClientLocationIds { get; set; } = string.Empty;
        public DateTime? DateOfServiceFrom { get; set; } = null;
        public DateTime? DateOfServiceTo { get; set; } = null;
        public DateTime? ClaimBilledFrom { get; set; } = null;
        public DateTime? ClaimBilledTo { get; set; } = null;
        public DateTime? ReceivedFrom { get; set; } = null;
        public DateTime? ReceivedTo { get; set; } = null;
        public DateTime? TransactionDateFrom { get; set; } = null;
        public DateTime? TransactionDateTo { get; set; } = null;
        public int? PatientId { get; set; } = 0;
        public int? ClaimStatusBatchId { get; set; } = 0;
        public string FlattenedLineItemStatus { get; set; } = null;

    }

    public class ExportDynamicClaimDashboardQueryHandler : IRequestHandler<ExportDynamicClaimDashboardQuery, Result<List<DynamicExportQueryResponse>>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;
        private readonly IStringLocalizer<ExportDynamicClaimDashboardQueryHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExcelService _excelService;

        private int _clientId => _currentUserService.ClientId;

        public ExportDynamicClaimDashboardQueryHandler(IStringLocalizer<ExportDynamicClaimDashboardQueryHandler> localizer, IClaimStatusQueryService claimStatusQueryService, ICurrentUserService currentUserService, IExcelService excelService)
        {
            _localizer = localizer;
            _claimStatusQueryService = claimStatusQueryService;
            _currentUserService = currentUserService;
            _excelService = excelService;
        }

        public async Task<Result<List<DynamicExportQueryResponse>>> Handle(ExportDynamicClaimDashboardQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _claimStatusQueryService.GetDynamicClaimExportDashboardQueryAsync(query, clientId: _clientId);
                return await Result<List<DynamicExportQueryResponse>>.SuccessAsync(response);
            }
            catch (Exception ex)
            {
                return await Result<List<DynamicExportQueryResponse>>.FailAsync(ex.Message);
            }
        }
    }
}
