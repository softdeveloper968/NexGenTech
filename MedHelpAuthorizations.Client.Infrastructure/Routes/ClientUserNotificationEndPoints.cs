namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    internal class ClientUserNotificationEndPoints
    {
        public static string Save = "api/v1/tenant/ClientUserNotification";

        public static string GetRecent(int maxResult)
        {
            return $"api/v1/tenant/ClientUserNotification/recent?maxResult={maxResult}";
        }

        public static string GetByFileName(string fileName)
        {
            return $"api/v1/tenant/ClientUserNotification/{fileName}";
        }
    }
}
