using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Dashboards.Queries.GetData;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Dashboards
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}