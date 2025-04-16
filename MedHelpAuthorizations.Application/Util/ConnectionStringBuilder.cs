using MedHelpAuthorizations.Domain.IdentityEntities.Enums;

namespace MedHelpAuthorizations.Application.Util
{
    public static class ConnectionStringBuilder
    {
        public static string GetConnectionString(string serverName, string databaseName, AuthenticationType authenticationType, string userName, string password)
        {
            if (authenticationType == AuthenticationType.Windows)
            {
                return $"Server={serverName};Initial Catalog={databaseName};Integrated Security=true;MultipleActiveResultSets=True;TrustServerCertificate=true; Connection Timeout=30; Command Timeout=720;";
            }
            else
            {
                return $"Server={serverName};Initial Catalog={databaseName};User ID={userName};Password={password};MultipleActiveResultSets=True;TrustServerCertificate=true; Connection Timeout=30; Command Timeout=720;";
            }
        }
    }
}
