namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class TokenEndpoints
    {
        public static string Get = "api/identity/token";
        public static string GetLogin = "api/identity/token/login";
        public static string Refresh = "api/identity/token/refresh";
        public static string RolesAndPermissions = "api/identity/token/roles-permissions";
        public static string ClientInfo = "api/identity/token/client";
        public static string SelectTenantClient = "api/identity/token/tenant-client";
        public static string Logout = "api/identity/token/logout";
    }
}