using MedHelpAuthorizations.Domain.Entities;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientUserNotificationRepository
    {
        IQueryable<ClientUserNotification> ClientUserNotifications { get; }
        Task<ClientUserNotification> GetNotificationByFileNameAsync(string fileName, int clientId);
        Task UpdateAsync(ClientUserNotification clientUserNotification);
    }
}
