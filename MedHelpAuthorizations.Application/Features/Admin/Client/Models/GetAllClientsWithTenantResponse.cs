using MedHelpAuthorizations.Domain.Entities.Enums;

namespace MedHelpAuthorizations.Application.Features.Admin.Client.Models
{
	public class GetAllClientsWithTenantResponse
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string ClientCode { get; set; }
        public long? PhoneNumber { get; set; }
        public long? FaxNumber { get; set; }
        public int TenantFailedConfigurationCount { get; set; }
        public SourceSystemEnum? SourceSystemId { get; set; }
        public int? TaxId { get; set; }
        public bool IsActive { get; set; }
    }
}
