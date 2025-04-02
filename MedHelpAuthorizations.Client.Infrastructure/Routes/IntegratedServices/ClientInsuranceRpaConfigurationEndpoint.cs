
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByRpaInsurance;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ClaimStatus.ClientInsuranceRpaConfigurations.Queries.GetByUserrnameAndUrl;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices
{
    public class ClientInsuranceRpaConfigurationEndpoint
    {
        public static string GetAll()
        {
            return $"api/v1/tenant/ClientInsuranceRpaConfiguration";
        }
       
        public static string UpdateCurrentClaimCount()
        {
            return $"api/v1/tenant/ClientInsuranceRpaConfiguration/UpdateCurrentClaimCount";
        }
        public static string GetById(int id)
        {
            //api/v1/tenant/clientInsuranceRpaConfiguration/3
            return $"api/v1/tenant/ClientInsuranceRpaConfiguration/{id}";
        }

        public static string GetSingleByCriteria(GetSingleClientInsuranceRpaConfigurationByCriteriaQuery query)
        {
            var queryString = $"api/v1/tenant/ClientInsuranceRpaConfiguration/SearchSingle?ClientId={query.ClientId}&RpaInsuranceId={query.RpaInsuranceId}&TransactionTypeId={query.TransactionTypeId}";
            if(query.AuthTypeId != null)
            {
                queryString = queryString + $"&AuthTypeId={query.AuthTypeId}";
            }
			if (query.ClientLocationId != null)
			{
				queryString = queryString + $"&ClientLocationId={query.ClientLocationId}";
			}
			return queryString;
        }

        public static string GetByRpaInsuranceId(GetClientInsuranceRpaConfigurationByRpaInsuranceQuery query)
        {
            return $"api/v1/tenant/ClientInsuranceRpaConfiguration/RpaInsurance?RpaInsuranceId={query.RpaInsuranceId}&TransactionTypeId={(int)query.TransactionTypeId}";
        }

        
        public static string GetByUsernameAndUrl(GetClientInsuranceRpaConfigurationsByUsernameAndUrlQuery query)
        {
            return $"api/v1/tenant/ClientInsuranceRpaConfiguration/SearchUsername?Username={query.Username}&TargetUrl={query.TargetUrl}&TransactionTypeId={(int)query.TransactionTypeId}";
        }

        public static string Save = "api/v1/tenant/ClientInsuranceRpaConfiguration";
        public static string Edit = "api/v1/tenant/ClientInsuranceRpaConfiguration";
        public static string Delete = "api/v1/tenant/ClientInsuranceRpaConfiguration";

        public static string ExpiryWarning = "api/v1/tenant/ClientInsuranceRpaConfiguration/UpdateExpiryWarning";
        public static string FailureReported = "api/v1/tenant/ClientInsuranceRpaConfiguration/UpdateFailureReported";

        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/ClientInsuranceRpaConfiguration/allPaginated?pageNumber={pageNumber}&pageSize={pageSize}";
        }
                    
        public static string GetErroredOrFailedCredentialConfig = $"api/v1/tenant/ClientInsuranceRpaConfiguration/GetErrorredOrFailedConfig";
		    
	}
}
