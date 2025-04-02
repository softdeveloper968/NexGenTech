namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class RolesEndpoints
    {
        public static string Delete = "api/admin/role";
        public static string GetAll = "api/admin/role";
        public static string Save = "api/admin/role";
        public static string GetPermissions(string id) => $"api/admin/role/{id}/permissions/";
        public static string UpdatePermissions = "api/admin/role/permissions/update";
    }
}