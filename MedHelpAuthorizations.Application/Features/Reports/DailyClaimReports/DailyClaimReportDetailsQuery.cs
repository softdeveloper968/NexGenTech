using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MedHelpAuthorizations.Shared;
using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.Base;

namespace MedHelpAuthorizations.Application.Features.Reports.DailyClaimReports
{
    public class DailyClaimReportDetailsQuery : ClaimStatusDashboardQueryBase, IRequest<PaginatedResult<DailyClaimStatusReportResponse>>
    {
        public int? ClientId = null;
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string SortType { get; set; } = "ASC";
        public string SearchText { get; set; } = "";
        public string FilterReportBy { get; set; }
        public int ClaimStatusBatchId { get; set; } = 0;

        //Used for when need to force switching contexts like we do for hangFire jobs that loop through each tenant
        public string ConnStr { get; }

        public DailyClaimReportDetailsQuery(int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, int? clientId = null, string connStr = null)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            StartDate = startDate;
            EndDate = endDate;
            ClientId = clientId;
            ConnStr = connStr;
        }
    }

    public class DailyClaimReportDetailsQueryHandler : IRequestHandler<DailyClaimReportDetailsQuery, PaginatedResult<DailyClaimStatusReportResponse>>
    {
        private readonly IClaimStatusQueryService _claimStatusQueryService;

        public DailyClaimReportDetailsQueryHandler(IClaimStatusQueryService claimStatusQueryService) => _claimStatusQueryService = claimStatusQueryService;

        public async Task<PaginatedResult<DailyClaimStatusReportResponse>> Handle(DailyClaimReportDetailsQuery query, CancellationToken cancellationToken)
        {
            var dailyClaimStatusReportList = new List<DailyClaimStatusReportResponse>();
            DateTime startDate = query.StartDate.Value;
            DateTime endDate = query.EndDate.Value;

            if (query.ReceivedFrom.HasValue)
            {
                startDate = query.ReceivedFrom.Value;
                endDate = query.ReceivedTo.Value;
            }
            else if (query.DateOfServiceFrom.HasValue)
            {
                startDate = query.DateOfServiceFrom.Value;
                endDate = query.DateOfServiceTo.Value;
            }
            else if (query.TransactionDateFrom.HasValue)
            {
                startDate = query.TransactionDateFrom.Value;
                endDate = query.TransactionDateTo.Value;
            }
            else if (query.ClaimBilledFrom.HasValue)
            {
                startDate = query.ClaimBilledFrom.Value;
                endDate = query.ClaimBilledTo.Value;
            }

            // Update the query to fetch data without pagination (for client-side pagination)
            var dates = DailyClaimReportHelper.GetDateRangeByPagination(startDate, endDate.AddDays(1), 1, int.MaxValue).ToList();
            if (!dates.Any())
            {
                return PaginatedResult<DailyClaimStatusReportResponse>.Failure(new List<string> { "Please verify Selected DateRange!!" });
            }

            // Build claim status query based on the complete date range
            var claimStatusQuery = ConfigureDailyClaimReportQuery(query, day: null, startDate: startDate, endDate: endDate);
            var result = await _claimStatusQueryService.GetDailyClaimsStatusTotalsAsync(claimStatusQuery, query.ConnStr);

            dates.ForEach(date =>
            {
                var claimStatusUploadedTotals = result.ClaimStatusUploadedTotals.Where(x => x.FilterDailyClaimReport.HasValue && x.FilterDailyClaimReport.Value.Date == date.Value.Date).ToList();
                var claimStatusTransactionTotals = result.ClaimStatusTransactionTotals.Where(x => x.FilterDailyClaimReport.HasValue && x.FilterDailyClaimReport.Value.Date == date.Value.Date).ToList();
                var claimStatusInProcessTotals = result.ClaimStatusInProcessTotals.Where(x => x.FilterDailyClaimReport.HasValue && x.FilterDailyClaimReport.Value.Date == date.Value.Date).ToList();
                var claimStatusDenialReasonTotals = result.DenialReasonTotals.Where(x => x.FilterDailyClaimReport.HasValue && x.FilterDailyClaimReport.Value.Date == date.Value.Date).ToList();

                var claimStatusDailyReport = _claimStatusQueryService.GetDailyClaimStatusReportResponse(date.Value, claimStatusUploadedTotals, claimStatusTransactionTotals, claimStatusInProcessTotals, claimStatusDenialReasonTotals);
                if (claimStatusDailyReport is not null)
                {
                    dailyClaimStatusReportList.Add(claimStatusDailyReport);
                }
            });

            dailyClaimStatusReportList = dailyClaimStatusReportList.OrderBy(z => z.ClaimBilledDate).ToList();
            int count = dailyClaimStatusReportList.Count;

            // Return full result for client-side pagination
            return PaginatedResult<DailyClaimStatusReportResponse>.Success(dailyClaimStatusReportList, count, 1, count);
        }


        private DailyClaimReportDetailsQuery ConfigureDailyClaimReportQuery(DailyClaimReportDetailsQuery query, DateTime? day, DateTime? startDate, DateTime? endDate)
        {
            if (query.ReceivedFrom.HasValue)
            {
                query.ReceivedFrom = day.HasValue ? day.Value : startDate.Value;
                query.ReceivedTo = day.HasValue ? day.Value : endDate.Value;
                query.FilterReportBy = "ReceivedDate";
            }
            else if (query.DateOfServiceFrom.HasValue)
            {
                query.DateOfServiceFrom = day.HasValue ? day.Value : startDate.Value;
                query.DateOfServiceTo = day.HasValue ? day.Value : endDate.Value;
                query.FilterReportBy = "DateOfService";
            }
            else if (query.TransactionDateFrom.HasValue)
            {

                query.TransactionDateFrom = day.HasValue ? day.Value : startDate.Value;
                query.TransactionDateTo = day.HasValue ? day.Value : endDate.Value;
                query.FilterReportBy = "TransactionDate";
            }
            else if (query.ClaimBilledFrom.HasValue)
            {
                query.ClaimBilledFrom = day.HasValue ? day.Value : startDate.Value;
                query.ClaimBilledTo = day.HasValue ? day.Value : endDate.Value;
                query.FilterReportBy = "ClaimBilledDate";
            }
            else
            {
                query.ClaimBilledFrom = day.HasValue ? day.Value : startDate.Value;
                query.ClaimBilledTo = day.HasValue ? day.Value : endDate.Value;
                query.FilterReportBy = "ClaimBilledDate";
            }

            return query;
        }
    }
}
