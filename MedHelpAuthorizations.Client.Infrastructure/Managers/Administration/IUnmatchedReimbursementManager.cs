using MedHelpAuthorizations.Application.Features.Administration.UnmatchedReimbursements.Queries;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IUnmatchedReimbursementManager : IManager
    {
        Task<PaginatedResult<GetAllUnmatchedReimbursementResponse>> GetAllUnmatchedReimbursementPagedAsync(GetAllUnmatchedReimbursementRequest request);
    }
}
