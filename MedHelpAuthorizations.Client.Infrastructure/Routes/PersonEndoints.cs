using MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsByCriteria;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class PersonEndoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/v1/tenant/persons?pageNumber={pageNumber}&pageSize={pageSize}";
        }
        public static string GetById(int id)
        {
            //api/v1/tenant/Persons/3
            return $"api/v1/tenant/persons/{id}";
        }

        public static string GetByCriteriaPaged(GetPersonsByCriteriaQuery query)
        {
            return $"api/v1/tenant/persons/Search?PageNumber={query.PageNumber}&PageSize={query.PageSize}&" +
                $"LastName={query.LastName}&FirstName={query.FirstName}&BirthDate={query.DateOfBirth?.ToString("yyyy-MM-dd")}&" +
                $"AddressStreetLine1={query.AddressStreetLine1}&City={query.City}&State={query.StateId}&PostalCode={query.PostalCode}";
        }

        public static string GetCount = "api/v1/tenant/persons/count";

        public static string GetPersonImage(int personId)
        {
            return $"api/v1/tenant/persons/image/{personId}";
        }

        public static string Save = "api/v1/tenant/persons";
        public static string Delete = "api/v1/tenant/persons";
        public static string Export = "api/v1/tenant/persons/export";
    }
}
