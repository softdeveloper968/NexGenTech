using MedHelpAuthorizations.Application.Features.ResponsibleParties.GetByCriteria;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
namespace MedHelpAuthorizations.Client.Infrastructure.Routes
{
    public class ResponsiblePartyEndPoints
    {
        public static string GetById(int id) => $"api/v1/tenant/ResponsibleParty/{id}";

        public static string GetByCriteria(GetByCritieriaResponsiblePartyQuery query) => $"api/v1/tenant/ResponsibleParty/Criteria?" + query.GetQueryString();

        public static string AccNumber(string accNumber) => $"api/v1/tenant/ResponsibleParty/account/{accNumber}";

        public static string ExternalId(string externalId) => $"api/v1/tenant/ResponsibleParty/externalid/{externalId}";

        public static string Save() => $"api/v1/tenant/ResponsibleParty";
        
    }
}
