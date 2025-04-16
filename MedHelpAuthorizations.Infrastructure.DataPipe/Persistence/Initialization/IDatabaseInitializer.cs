using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Initialization
{
    public interface IDatabaseInitializer
    {
        Task InitializeDatabasesAsync(CancellationToken cancellationToken);
        Task InitializeDataPipeDbAsync(CancellationToken cancellationToken);
    }
}
