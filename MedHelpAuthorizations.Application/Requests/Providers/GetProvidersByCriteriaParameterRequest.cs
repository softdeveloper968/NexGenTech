using MedHelpAuthorizations.Shared.Requests;
using System;

namespace MedHelpAuthorizations.Application.Requests.Providers
{
    public class GetProvidersByCriteriaParameterRequest : PagedRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
