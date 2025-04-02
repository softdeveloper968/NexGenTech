
using MedHelpAuthorizations.Application.Features.Providers.GetByCriteria;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class ProviderEndPoints
    {
        public static string Save = "api/v1/tenant/provider";

        public static string Delete = "api/v1/tenant/Provider";

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"api/v1/tenant/Provider/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }
        
        public static string GetAllClientProviders = "api/v1/tenant/Provider/AllClientProviders";
        public static string GetById(int id) => $"api/v1/tenant/Provider/{id}";
        public static string GetByCriteria(GetProvidersByCriteriaQuery query) => $"api/v1/tenant/Provider/Criteria?" + query.GetQueryString();

        public static string GetAverageDaysByProviders = "api/v1/tenant/Provider/GetAvgDaysToPayByProvider"; //EN-190
        public static string ChargesByProvider = "api/v1/tenant/Provider/ChargesByProvider"; //EN-190
        public static string ProceduresByProvider = "api/v1/tenant/Provider/ProceduresByProvider"; //EN-241
        public static string InsurancesByProvider = "api/v1/tenant/Provider/InsurancesTotalsByProvider"; //EN-250
        public static string DenialReasonsByProvider = "api/v1/tenant/Provider/GetDenialReasonsByProvider"; //EN-250
        public static string GetProcedureReimbursementByProvider = "api/v1/tenant/Provider/GetProcedureReimbursementByProvider"; //EN-254
        public static string GetPayerReimbursementByProvider = "api/v1/tenant/Provider/GetPayerReimbursementByProvider"; //EN-257
        public static string GetProviderProcedureTotal = "api/v1/tenant/Provider/ProviderProcedureTotal"; 
        public static string GetDenialReasonTotalsByProviderId = "api/v1/tenant/Provider/GetDenialReasonTotalsByProviderId"; 
    }
}
