using MedHelpAuthorizations.Application.Features.MonthClose.Queries;
using MedHelpAuthorizations.Domain.Entities;
using System.Threading.Tasks;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.MonthClose
{
    public interface IMonthCloseManager : IManager
    {
        Task<PaginatedResult<MonthlyCashCollectionData>> GetMonthlyCashCollectionDataAsync(IMonthCloseDashboardQuery request);
        Task<PaginatedResult<MonthlyDenialData>> GetMonthlyDenialDataAsync(IMonthCloseDashboardQuery request);
        Task<PaginatedResult<MonthlyReceivablesData>> GetMonthlyReceivablesDataAsync(IMonthCloseDashboardQuery request);
        Task<PaginatedResult<MonthlyARData>> GetMonthlyARDataAsync(IMonthCloseDashboardQuery request);
    }
}
