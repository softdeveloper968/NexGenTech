using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Base;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Base;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Base
{
    public class ClientLocationInsuranceIdentifierDto
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ClientLocationId { get; set; }

        public int ClientInsuranceId { get; set; }

        public string Identifier { get; set; }

        public ClientLocationDto ClientLocation { get; set; }

        public ClientInsuranceDto ClientInsurance { get; set; }
    }
}
