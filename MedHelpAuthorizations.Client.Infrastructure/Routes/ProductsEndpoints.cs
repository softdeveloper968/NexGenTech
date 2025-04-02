namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class ProductsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/products?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string GetCount = "api/v1/tenant/products/count";

        public static string GetProductImage(int productId)
        {
            return $"api/v1/tenant/products/image/{productId}";
        }

        public static string Save = "api/v1/tenant/products";
        public static string Delete = "api/v1/tenant/products";
        public static string Export = "api/v1/tenant/products/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";
    }
}