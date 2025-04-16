namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public static class RpaInsurancesEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/RpaInsurance?pageNumber={pageNumber}&pageSize={pageSize}";
        }
        public static string GetAll(string searchString)
        {
            return $"api/v1/tenant/RpaInsurance?&searchString={searchString}";
        }
        //public static string GetById(int id)
        //{
        //    return $"api/v1/tenant/RpaInsurance/{id}";
        //}

        //public static string GetCount = "api/v1/tenant/RpaInsurance/count";
        public static string Save = "api/v1/tenant/RpaInsurance";
        //public static string Delete = "api/v1/tenant/RpaInsurance";
        //public static string Export = "api/v1/tenant/RpaInsurance/export";
    }
}