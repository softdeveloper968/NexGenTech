using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization
{
    public interface IDatabaseInitializer
    {
        Task InitializeDatabasesAsync(CancellationToken cancellationToken);
        Task InitializeApplicationDbForTenantAsync(AitTenantInfo tenant, CancellationToken cancellationToken);
    }
}
