namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public static class InputDocumentsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/inputdocuments?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/inputdocuments/{id}";
        }

        //public static string GetByCriteriaPaged(int patientId, int authId, int pageNumber, int pageSize)
        //{
        //    return $"api/tenant/inputdocuments/criteria?pageNumber={pageNumber}&pageSize={pageSize}";
        //}

        public static string Save = "api/v1/tenant/inputdocuments";
        public static string SaveImportDocument = "api/v1/tenant/inputdocuments/process";
        public static string Delete = "api/v1/tenant/inputdocuments";
		public static string RetryImportDocument = "api/v1/tenant/inputdocuments/retry";
	}
}