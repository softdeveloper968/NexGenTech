using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class UserAlertRepositoryAysnc : RepositoryAsync<UserAlert, int>, IUserAlertRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserAlertRepositoryAysnc(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<UserAlert> UserAlerts => _dbContext.UserAlerts;

        public async Task<bool> DoScheduledCleanup()
        {
            try
            {
                if (await _dbContext.UserAlerts.AnyAsync(x => DateTime.Now > x.CreatedOn.AddDays(30)))
                {
                    var expired = _dbContext.UserAlerts.Where(x => DateTime.Now > x.CreatedOn.AddDays(30));
                    _dbContext.UserAlerts.RemoveRange(expired);
                    var affected = await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                
            }
            return true;
        }
    }
}
