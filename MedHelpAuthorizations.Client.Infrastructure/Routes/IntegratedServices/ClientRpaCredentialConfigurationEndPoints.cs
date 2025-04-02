namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public class ClientRpaCredentialConfigurationEndPoints
    {
        public static string CreateCredentialConfig = $"api/v1/tenant/ClientRpaCredentialConfiguration/CreateCredentialConfig";
        public static string UpdateCredentialConfig = $"api/v1/tenant/ClientRpaCredentialConfiguration/UpdateCredentialConfig";
        public static string GetCredentialConfig = $"api/v1/tenant/ClientRpaCredentialConfiguration/GetCredentialConfig";
        public static string ResetCredentialConfig = $"api/v1/tenant/ClientRpaCredentialConfiguration/ResetCredentialConfig";
        public static string UpdateIsCredentialInUse()
        {
            return $"api/v1/tenant/ClientRpaCredentialConfiguration/UpdateIsCredentialInUse";
        }
        public static string GetRpaConfigurationsWithLocation = $"api/v1/tenant/ClientRpaCredentialConfiguration/GetRpaConfigurationsWithLocation";
    }
}
