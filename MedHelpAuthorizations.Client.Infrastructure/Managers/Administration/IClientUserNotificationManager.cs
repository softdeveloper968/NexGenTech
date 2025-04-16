using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Commands;
using MedHelpAuthorizations.Application.Features.Administration.ClientUserNotifications.Queries.GetAllPaged;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientUserNotificationManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditClientUserNotificationCommand request);
        Task<IResult<List<GetAllClientUserNotificationResponse>>> GetRecentClientsAsync(int maxResults);
        Task<IResult<GetAllClientUserNotificationResponse>> GetNotificationByFileNameAsync(string fileName);
    }
}
