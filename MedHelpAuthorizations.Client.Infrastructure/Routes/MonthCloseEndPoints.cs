using MedHelpAuthorizations.Application.Features.MonthClose.Queries;
using MedHelpAuthorizations.Application.Interfaces.Services;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class MonthCloseEndPoints
    {
        // Endpoint to get Monthly Cash Collection Data by passing clientId, location, provider, insurance, and CPT codes

        public static string GetMonthlyCashCollectionData(IMonthCloseDashboardQuery request)
        {
            var url = $"api/v1/tenant/MonthClose/cash?clientId={request.ClientId}&clientLocationIds={request.ClientLocationId}&clientProviderIds={request.ClientProviderId}&clientInsuranceIds={request.ClientInsuranceId}&cptCodeIds={request.CptCodeId}";
            return url;
        }

        public static string GetMonthlyReceivablesData(IMonthCloseDashboardQuery request)
        {
            var url = $"api/v1/tenant/MonthClose/receivables?clientId={request.ClientId}&clientLocationIds={request.ClientLocationId}&clientProviderIds={request.ClientProviderId}&clientInsuranceIds={request.ClientInsuranceId}&cptCodeIds={request.CptCodeId}";
            return url;
        }

        public static string GetMonthlyARData(IMonthCloseDashboardQuery request)
        {
            var url = $"api/v1/tenant/MonthClose/ar?clientId={request.ClientId}&clientLocationIds={request.ClientLocationId}&clientProviderIds={request.ClientProviderId}&clientInsuranceIds={request.ClientInsuranceId}&cptCodeIds={request.CptCodeId}";
            return url;
        }

        public static string GetMonthlyDenialData(IMonthCloseDashboardQuery request)
        {
            var url = $"api/v1/tenant/MonthClose/denial?clientId={request.ClientId}&clientLocationIds={request.ClientLocationId}&clientProviderIds={request.ClientProviderId}&clientInsuranceIds={request.ClientInsuranceId}&cptCodeIds={request.CptCodeId}";
            return url;
        }
    }
}
