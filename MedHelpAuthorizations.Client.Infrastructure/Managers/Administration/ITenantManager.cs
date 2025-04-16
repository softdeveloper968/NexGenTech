using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface ITenantManager : IManager
    {
        Task<IResult<List<BasicTenantInfoResponse>>> GetAllAsync();
    }
}