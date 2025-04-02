using MedHelpAuthorizations.Application.Models.Common;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Models
{
    public class TenantInfoResponse : AuditableResponse
    {
        public int TenantId { get; set; }
        public int ServerId { get; set; }
        public string ServerName { get; set; }
        public string TenantIdentifier { get; set; }
        public string TenantName { get; set; }
        public string DatabaseName { get; set; }
        public string AdminEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidUpto { get; set; }
        public int TenantFailedConfigurationCount { get; set; }
        public bool IsProductionTenant { get; set; }
    }
}
