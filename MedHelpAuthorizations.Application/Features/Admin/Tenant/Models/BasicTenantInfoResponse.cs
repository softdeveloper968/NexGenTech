
namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Models
{
    public class BasicTenantInfoResponse
    {
        public int TenantId { get; set; }
        public string TenantName { get; set; }

        public string TenantIdentifier { get; set; }    
    }
}
