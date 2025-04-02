using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class ClientUserNotificationRepository : RepositoryAsync<ClientUserNotification, int>, IClientUserNotificationRepository
    {
        private readonly ApplicationContext _dbContext;
        public ClientUserNotificationRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<ClientUserNotification> ClientUserNotifications => _dbContext.ClientUserNotifications;

        public async Task<ClientUserNotification> GetNotificationByFileNameAsync(string fileName, int clientId)
        {
            try
            {
                // Retrieve the notification based on the file name and client ID
                return await _dbContext.ClientUserNotifications
                    .FirstOrDefaultAsync(notification => notification.FileName == fileName && notification.ClientId == clientId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task UpdateAsync(ClientUserNotification clientUserNotification)
        {
            _dbContext.ClientUserNotifications.Update(clientUserNotification);
            await _dbContext.SaveChangesAsync();
        }

    }
}
