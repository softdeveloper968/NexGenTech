namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientEncounterTypeEndPoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/ClientEncounterType/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string Save = "api/v1/tenant/ClientEncounterType";

        public static string Delete = "api/v1/tenant/ClientEncounterType";
    }
}
