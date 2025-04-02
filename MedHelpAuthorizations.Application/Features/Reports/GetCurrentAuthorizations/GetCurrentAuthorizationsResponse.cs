using MedHelpAuthorizations.Application.Features.Authorizations.Queries.GetById;
using System;

namespace MedHelpAuthorizations.Application.Features.Reports.GetCurrentAuthorizations
{
    public class GetCurrentAuthorizationsResponse : GetAuthorizationByIdResponse
    {
        public string InsuranceName { get; set; }
        public string InsurancePolicyNumber { get; set; }
        public long? InsurancePhoneNumber { get; set; }
        public long? InsuranceFaxNumber { get; set; }
        public string Notes { get; set; }
        public string CareManagerName { get; set; }
        public DateTime? CallbackDate { get; set; } = null;
    }
}
