namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class UserClientEndpoints
    {
        internal static string GetClientsByUserIdPaged(int pageNumber, int pageSize, string userId)
        {
            return $"api/v1/tenant/UserClient/ByUser?pageNumber={pageNumber}&pageSize={pageSize}&userId={userId}";
        }      

        public static string Save = "api/v1/tenant/UserClient";
    }
}
