using MedHelpAuthorizations.Application.Options;
using Microsoft.Extensions.Options;
namespace MedHelpAuthorizations.Infrastructure.DataPipe.Persistence.Initialization
{
    internal class DatabaseSettingsDesignTimeFactoryOptions : IOptions<DatabaseSettings>
    {
        public DatabaseSettings Value { get; set; }
    }
}
