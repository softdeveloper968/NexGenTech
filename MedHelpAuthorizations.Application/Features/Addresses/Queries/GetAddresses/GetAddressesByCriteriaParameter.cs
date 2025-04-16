using System;
using MedHelpAuthorizations.Application.Parameters;

namespace MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAddresses
{
    public class GetAddressesByCriteriaParameter : RequestParameter
    {
        public string AddressLine1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
    }
}
