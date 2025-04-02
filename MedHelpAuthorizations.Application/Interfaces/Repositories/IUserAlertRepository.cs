using MedHelpAuthorizations.Domain.Entities;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IUserAlertRepository : IRepositoryAsync<UserAlert, int>
    {
        IQueryable<UserAlert> UserAlerts { get; }
        Task<bool> DoScheduledCleanup();
    }
}