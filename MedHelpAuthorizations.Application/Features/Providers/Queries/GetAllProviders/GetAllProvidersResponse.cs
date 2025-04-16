using MedHelpAuthorizations.Application.Features.Providers.Queries.Base;
using MedHelpAuthorizations.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders
{
    public class GetAllProvidersResponse : GetProvidersResponseBase
    {
        public List<ClientLocation> Locations { get; set; }
        public string ProviderLocationNames => (Locations is not null && Locations.Any()) ? string.Join(",", Locations.Select(z => z.Name).ToList()) : $"";
    }
}
