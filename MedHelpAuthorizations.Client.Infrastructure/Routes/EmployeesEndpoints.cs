namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    class EmployeesEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/Employee?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string GetAllManagers()
        {
            return "api/v1/tenant/Employee/managers";
        }

        public static string Save = "api/v1/tenant/Employee";

        public static string Delete = "api/v1/tenant/Employee";

        public static string GetAllEmployees()
        {
            return "api/v1/tenant/Employee";
        }

		public static string GetEmployeeById(int id)
        {
            return $"api/v1/tenant/Employee/{id}";
        }
        public static string GetAllEmployeeData = "api/v1/tenant/Employee/EmployeeData";
    }
}
