using MedHelpAuthorizations.Application.Features.Reports.DailyClaimReports;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Shared.Wrapper;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Reports.DailyMonthlyReports
{
    public interface IDailyClaimReportManager : IManager
    {
        Task<PaginatedResult<DailyClaimStatusReportResponse>> GetDailyClaimReportByCriteria(DailyClaimReportDetailsQuery dailyClaimReportDetails);

    }
}
