namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class UnmatchedReimbursementEndPoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"api/v1/tenant/UnmatchedReimbursement/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }
    }
}
