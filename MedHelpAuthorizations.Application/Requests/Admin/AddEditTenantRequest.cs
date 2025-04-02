namespace MedHelpAuthorizations.Application.Requests.Admin
{
    public class AddEditTenantRequest
    {
        public int TenantId { get; set; }
        public int DatabaseServerId { get; set; }
        public string TenantIdentifier { get; set; }
        public string TenantName { get; set; }
        public string DatabaseName { get; set; }
        public string AdminEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidUpto { get; set; }
        public bool IsProductionTenant {  get; set; }
    }
}
