using MedHelpAuthorizations.Application.Options;
using Microsoft.Extensions.Options;

namespace MedHelpAuthorizations.Infrastructure.Identity.Persistence.Initialization
{
    public class DatabaseSettingsDesignTimeFactoryOptions : IOptions<DatabaseSettings>
    {
        public DatabaseSettings Value { get; set; }
    }
}
