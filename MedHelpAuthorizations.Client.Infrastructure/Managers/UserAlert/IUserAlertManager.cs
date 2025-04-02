using MedHelpAuthorizations.Application.Features.UserAlerts.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetById;
using MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetByUserId;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.UserAlert
{
    public interface IUserAlertManager : IManager
    {
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<GetUserAlertsByIdResponse>> GetByIdAsync(GetUserAlertByIdQuery request);
        Task<PaginatedResult<GetUserAlertByUserIdResponse>> GetUserAlertsByUserAsync(string userId, int pageNumber, int pageSize);
        Task<IResult<int>> SaveAsync(AddEditUserAlertCommand request);
    }
}