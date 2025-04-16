using MedHelpAuthorizations.Application.Features.Patients.Queries.GetPatientsByCriteria;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class QuestionnairesEndpoints
    {
        public static string GetAllPagedForPatient(int pageNumber, int pageSize, int patientId)
        {
            return $"api/v1/tenant/questionnaires/patient?patientId={patientId}&pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/questionnaires?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string GetById(int id)
        {
            return $"api/v1/tenant/questionnaires/{id}";
        }       

        public static string GetCount = "api/v1/tenant/questionnaires/count";
        public static string Save = "api/v1/tenant/questionnaires";
        public static string SavePatientQuestionnaire = "api/v1/tenant/questionnaires/patient";
        public static string Delete = "api/v1/tenant/questionnaires";
        public static string DeletePatientQuestionnaire = "api/v1/tenant/questionnaires/patient";
        public static string Export = "api/v1/tenant/questionnaires/export";
    }
}