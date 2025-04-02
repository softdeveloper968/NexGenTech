using MedHelpAuthorizations.Application.Features.InsuranceCards.Queries.GetByCriteria;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class InsuranceCardEndPoints
    {
        public static string GetAll() => $"api/v1/tenant/InsuranceCards";
        public static string GetById(int id) => $"api/v1/tenant/InsuranceCards/{id}";
        public static string GetByPatientId(int patientId) => $"api/v1/tenant/InsuranceCards/patientId/{patientId}";
        public static string GetByCardholderId(int cardholderId) => $"api/v1/tenant/InsuranceCards/cardholderid/{cardholderId}";
        public static string GetByCriteria(GetInsuranceCardsByCriteriaPagedQuery query) => $"api/v1/tenant/InsuranceCards/Search?" + query.GetQueryString();
        public static string Save() => $"api/v1/tenant/InsuranceCards";
        public static string Delete(int id) => $"api/v1/tenant/InsuranceCards/{id}";
    }
}
