using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;

namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class CardholderEndpoints
    {
        public static string GetById(int id) => $"api/v1/tenant/Cardholders/{id}";

        public static string GetBySearchString(string searchString) => $"api/v1/tenant/Cardholders/Search/{searchString}";

        public static string GetByCriteria(GetCardholdersByCriteriaQuery query)
        {
            //TODO: add in missing query parameters
            return $"api/v1/tenant/Cardholders/Criteria?PageNumber={query.PageNumber}&PageSize={query.PageSize}&" +
                $"LastName={query.LastName}&FirstName={query.FirstName}&BirthDate={query.BirthDate?.ToString("yyyy-MM-dd")}";
        } 
        
        public static string Save() => $"api/v1/tenant/Cardholders";

        public static string Delete(int id) => $"api/v1/tenant/Cardholders/{id}";
    }
}
