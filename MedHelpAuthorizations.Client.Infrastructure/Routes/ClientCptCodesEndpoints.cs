namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ClientCptCodesEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"api/v1/tenant/ClientCptCode?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }
        public static string GetById(int id)
        {
            return $"api/v1/tenant/ClientCptCode/{id}";
        }

        public static string GetCount = "api/v1/tenant/ClientCptCode/count";
        public static string Save = "api/v1/tenant/ClientCptCode";
        public static string Delete = "api/v1/tenant/ClientCptCode";
        public static string Export = "api/v1/tenant/ClientCptCode/export";
        public static string GetCptByClientId()
        {
            return $"api/v1/tenant/ClientCptCode/clientId";
        }

		public static string GetByCode(string code)
		{
			return $"api/v1/tenant/ClientCptCode/code?code={code}";
		}
        
        public static string CheckMatchCpt(int id)
		{
			return $"api/v1/tenant/ClientCptCode/cptCodeId?id={id}";
		}

		public static string CptCodeBySearch(string searchString)
		{
			return $"api/v1/tenant/ClientCptCode/search?searchString={searchString}";
		}

        public static string ProvidersByProcedure = "api/v1/tenant/ClientCptCode/ProviderByProcedure";
        public static string InsuranceByProcedure = "api/v1/tenant/ClientCptCode/InsuranceByProcedure";
        public static string DenialReasonsByProcedure = "api/v1/tenant/ClientCptCode/DenialReasonsByProcedure";
        public static string PayerReimbursementByProcedure = "api/v1/tenant/ClientCptCode/PayerReimbursementByProcedure";
        public static string ProviderReimbursementByProcedureCode = "api/v1/tenant/ClientCptCode/ProviderReimbursementByProcedureCode";
        public static string ReimbursementByProcedureCode = "api/v1/tenant/ClientCptCode/ReimbursementByProcedureCode";
        public static string ChargesByProcedureCode = "api/v1/tenant/ClientCptCode/ChargesByProcedureCode";
        public static string AverageDaysToPayByProcedureCodeQuery = "api/v1/tenant/ClientCptCode/AverageDaysToPayByProcedureCodeQuery";
        public static string ProviderTotalsByProcedureCode = "api/v1/tenant/ClientCptCode/ProviderTotalsByProcedureCode";

    }
}
