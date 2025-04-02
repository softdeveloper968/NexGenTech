using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Shared.Enums;
using System.Text;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public static class AuthorizationsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"api/v1/tenant/authorizations?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }
        public static string GetByCriteriaPaged(GetByCriteriaPagedAuthorizationsQuery request)
        {
            StringBuilder builder = new StringBuilder();
            if (request.AuthorizationStatuses != null)
            {
                foreach (var status in request.AuthorizationStatuses)
                {
                    builder.Append($"&AuthorizationStatuses={status}");
                }
            }
            if (request.AuthTypeNames != null)
            {
                foreach (var name in request.AuthTypeNames)
                {
                    builder.Append($"&AuthTypeNames={name}");
                }
                builder.Append($"&AuthTypeNamesFilterType={request.AuthTypeNamesFilterType}");
            }
            return $"api/v1/tenant/authorizations/Criteria?pageNumber={request.PageNumber}&pageSize={request.PageSize}&QueryStateType={request.QueryStateType}{builder.ToString()}";
        }
        public static string GetById(int id)
        {
            return $"api/v1/tenant/authorizations/{id}";
        }

        public static string GetByPatientIdPaged(int patientid, int pagenumber, int pagesize)
        {
            return $"api/v1/tenant/authorizations/SearchByPatient?PatientId={patientid}&PageNumber={pagenumber}&PageSize={pagesize}";
        }

        public static string GetCount = "api/v1/tenant/authorizations/count";

        public static string GetAuthorizationImage(int authorizationId)
        {
            return $"api/v1/tenant/authorizations/image/{authorizationId}";
        }
       

        public static string Save = "api/v1/tenant/authorizations";
        public static string Delete = "api/v1/tenant/authorizations";
        public static string Export = "api/v1/tenant/authorizations/export";
    }
}