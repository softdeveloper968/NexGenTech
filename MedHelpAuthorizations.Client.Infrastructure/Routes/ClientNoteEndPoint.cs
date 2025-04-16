namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientNoteEndPoint
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"api/v1/tenant/ClientNote/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }
        public static string Save = "api/v1/tenant/ClientNote";
        public static string Delete = "api/v1/tenant/ClientNote";
    }
}
