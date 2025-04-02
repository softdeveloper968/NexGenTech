using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.CustomDashboard
{
    public interface ICustomDashboardManager : IManager
    {
        Task<IResult<IEnumerable<DashboardItem>>> GetDashboardItems();
        Task<IResult<IEnumerable<UserDashboardItem>>> GetUserDashboardItems();
        Task<IResult> SaveDashboardConfigurations(List<UserDashboardItem> UserDashboardItems);
    }
}
