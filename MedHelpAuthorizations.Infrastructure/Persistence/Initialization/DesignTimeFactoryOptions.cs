using MedHelpAuthorizations.Application.Options;
using Microsoft.Extensions.Options;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization
{
    internal class DatabaseSettingsDesignTimeFactoryOptions : IOptions<DatabaseSettings>
    {
        public DatabaseSettings Value { get; set; }
    }
}
