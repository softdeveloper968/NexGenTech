using MedHelpAuthorizations.Application.Features.Administration.UnMappedClientFeeSchedule.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IUnmappedFeeScheduleCptManager : IManager
    {
        Task<PaginatedResult<GetAllUnmappedFeeScheduleResponse>> GetAllUnmappedFeeSchduleCptPagedAsync(GetAllUnmappedFeeScheduleCptRequest request);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<int>> CleanUpUnmappedFeeScheduleCptAsync(); //EN-156
    }
}
