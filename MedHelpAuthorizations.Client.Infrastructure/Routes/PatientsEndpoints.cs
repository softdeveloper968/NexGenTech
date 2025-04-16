using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class PatientsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/patients?pageNumber={pageNumber}&pageSize={pageSize}";
        }
        public static string GetById(int id)
        {
            //api/v1/tenant/Patients/3
            return $"api/v1/tenant/patients/{id}";
        }

        public static string GetBySearchString(string searchString) => $"api/v1/tenant/patients/SearchString/{searchString}";

        public static string GetByCriteriaPaged(GetPatientsByCriteriaQuery query)
        {
            //TODO: add in missing query parameters
            return $"api/v1/tenant/patients/Search?PageNumber={query.PageNumber}&PageSize={query.PageSize}&ExternalId={query.ExternalId}&AccountNumber={query.AccountNumber}&" +
                $"LastName={query.LastName}&FirstName={query.FirstName}&BirthDate={query.BirthDate?.ToString("yyyy-MM-dd")}" +
                $"&IsActive={query.IsActive}&IsAddedThisMonth={query.IsAddedThisMonth}&BenefitsNotChecked={query.BenefitsNotChecked}";
        }

        public static string GetCount = "api/v1/tenant/patients/count";

        public static string GetPatientImage(int patientId)
        {
            return $"api/v1/tenant/patients/image/{patientId}";
        }

        public static string Save = "api/v1/tenant/patients";
        public static string Delete = "api/v1/tenant/patients";
        public static string Export = "api/v1/tenant/patients/export";
    }
}