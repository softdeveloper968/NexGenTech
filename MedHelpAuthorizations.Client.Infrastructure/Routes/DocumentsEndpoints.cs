namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class DocumentsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/documents?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/documents/{id}";
        }

        public static string GetByCriteriaPaged(int patientId, int authId, int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/documents/criteria?pageNumber={pageNumber}&pageSize={pageSize}&patientId={patientId}&authorizationId={authId}";
        }

        public static string Save = "api/v1/tenant/documents";
        public static string Delete = "api/v1/tenant/documents";
    }
}