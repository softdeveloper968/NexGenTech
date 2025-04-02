using MedHelpAuthorizations.Shared.Helpers;
using System.Text.Json.Serialization;

namespace MedHelpAuthorizations.Application.Configurations
{
    public class StartupSettings
    {
        public bool SeedDatabase { get; set; } = true;
        [JsonPropertyName("EnvironmentType")]
        public string EnvironmentType { get; set; } = TenantHelper.BetaEnvironmentType;
    }
}
