using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Application.Features.Reports.ARAgingReport;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.ARAgingReports
{
    public interface IARAgingReportSummaryManager : IManager
    {
        Task<IResult<List<ARAgingSummaryReportResponse>>> GetARAgingSummaryClaimReportByCriteria(ARAgingSummaryClaimReportDetailsQuery claimReportDetails);
        Task<IResult<ARAgingDataResponse>> GetARAgingTotalsByCriteria(ARAgingDataQuery claimReportDetails);
        Task<string> ExportReverseAnalysisReport(ARAgingDataQuery query);
    }
}
