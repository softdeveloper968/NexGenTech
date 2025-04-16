namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class UserEndpoints
    {
        public static string GetAll = "api/admin/user";

        public static string GetAllWithPermission(string permission) => $"api/admin/user/permission/{permission}";

        public static string Get(string userId)
        {
            return $"api/admin/user/{userId}";
        }

        public static string GetUserRoles(string userId)
        {
            return $"api/admin/user/roles/{userId}";
        }

        public static string GetAssignedClients(string tenantIdentifier)
        {
            return $"api/admin/user/{tenantIdentifier}/clients";
        }

        public static string Register = "api/admin/user";
        public static string ToggleUserStatus = "api/admin/user/toggle-status";
        public static string ForgotPassword = "api/admin/user/forgot-password";
        public static string ResetPassword = "api/admin/user/reset-password";
        public static string GetUserTenants(string userId) => $"api/admin/user/{userId}/tenants";
        //public static string Tenants = "api/identity/user/tenants";
    }
}