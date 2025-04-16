using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Requests.Admin.Client
{
    public class AddEditAdminClientRequest
    {
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public long? PhoneNumber { get; set; }
        public long? FaxNumber { get; set; }
        public List<int> AuthTypes { get; set; }
        public List<int> Features { get; set; }
        public int? SourceSystemId { get; set; }
		public List<SpecialtyEnum> SpecialitIds { get; set; }
        public int? TaxId { get; set; }
        public bool IsActive { get; set; }
    }
}
