namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class UnmappedFeeScheduleEndPoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"api/v1/tenant/UnmappedFeeScheduleCpt/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }

        public static string Delete = "api/v1/tenant/UnmappedFeeScheduleCpt";

        public static string CleanUpUnmappedFeeScheduleCpt = "api/v1/tenant/UnmappedFeeScheduleCpt/cleanup";
    }
}
