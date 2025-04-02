using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Interfaces.Services.Admin
{
    public interface ITenantMediatorService
    {
        Task InitializeTenant(Domain.IdentityEntities.Tenant tenant, CancellationToken cancellationToken);
    }
}
