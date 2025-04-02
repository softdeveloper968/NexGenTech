using System.Linq;

namespace MedHelpAuthorizations.Domain.Entities
{
    public partial class ClientLocation
    {       
        public ClientLocationInsuranceIdentifier GetLocationIdentifierForInsuranceId(ClientLocation clientLocation, int? clientInsuranceId)
        {
            if (clientLocation.ClientLocationInsuranceIdentifiers.Any() && clientInsuranceId != null)
            {
                return clientLocation.ClientLocationInsuranceIdentifiers
                    .FirstOrDefault(cli => cli.ClientInsuranceId == clientInsuranceId) ?? new ClientLocationInsuranceIdentifier();
                    
            }

            return new ClientLocationInsuranceIdentifier();
        }

        public string GetLocationIdentifierStringForInsuranceId(ClientLocation clientLocation, int? clientInsuranceId)
        {
            if (clientInsuranceId != null && clientLocation != null && clientLocation.ClientLocationInsuranceIdentifiers.Any())
            {
                var locationInsuranceIdentifier = clientLocation.ClientLocationInsuranceIdentifiers.FirstOrDefault(cli => cli.ClientInsuranceId == clientInsuranceId);

                return locationInsuranceIdentifier?.Identifier;
            }

            return null;
        }
    }
}
