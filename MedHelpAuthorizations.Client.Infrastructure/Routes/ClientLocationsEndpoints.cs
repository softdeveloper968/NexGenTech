namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    internal class ClientLocationsEndpoints
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ClientLocation";
        }
        //AA-106
        public static string GetAllByProviderId(int providerId)
        {
            return $"api/v1/tenant/ClientLocation/getByProviderId?providerId={providerId}";
        }
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/ClientLocation/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}";
        }
        public static string GetBySearchString(string searchString)
        {
            return $"api/v1/tenant/ClientLocation?searchString={searchString}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/ClientLocation/{id}";
        }


        public static string GetByClientId(int clientId)
        {
            return $"api/v1/tenant/ClientLocation/getByClientId/{clientId}";
        }
        public static string GetRpaAssigned = "api/v1/tenant/ClientLocation/rpaAssigned";
        public static string GetCount = "api/v1/tenant/ClientLocation/count";
        public static string Save = "api/v1/tenant/ClientLocation";
        public static string Delete = "api/v1/tenant/ClientLocation";
        public static string Export = "api/v1/tenant/ClientLocation/export";

        public static string GetProcedureTotalsByLocation = "api/v1/tenant/ClientLocation/getProcedureTotals"; //EN-312
        public static string GetInsuranceTotalsByLocation = "api/v1/tenant/ClientLocation/getInsuranceTotals"; //EN-312
        public static string GetDenialReasonsByLocation = "api/v1/tenant/ClientLocation/getDenialTotals"; //EN-312
        public static string GetProcedureReimbursementByLocation = "api/v1/tenant/ClientLocation/getProcedureReimbursement"; //EN-312
        public static string GetPayerReimbursementByLocation = "api/v1/tenant/ClientLocation/getPayerReimbursement"; //EN-312
        public static string GetAverageDaysToPayByLocation = "api/v1/tenant/ClientLocation/getAverageDaysToPay"; //EN-312
        public static string GetCharges = "api/v1/tenant/ClientLocation/getCharges"; //EN-312
    }
}
