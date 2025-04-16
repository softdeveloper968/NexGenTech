namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    class EmployeeClientsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/EmployeeClient?pageNumber={pageNumber}&pageSize={pageSize}";
        }


        public static string GetEmployeeClientViewModelsByClientId(int clientId)
        {
            return $"api/v1/tenant/EmployeeClient/Client/{clientId}";
        }

        public static string GetAllByClientId(int clientId)
        {
            return $"api/v1/tenant/EmployeeClient?/Client/{clientId}";
        }

        public static string Save = "api/v1/tenant/EmployeeClient";

        public static string Delete = "api/v1/tenant/EmployeeClient";

        public static string GetAllEmployeeClients()
        {
            return "api/v1/tenant/EmployeeClient";
        }

        public static string GetEmployeeClientById(int id)
        {
            return $"api/v1/tenant/EmployeeClient/{id}";
        }
    }
}
