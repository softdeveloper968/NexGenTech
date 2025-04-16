
namespace MedHelpAuthorizations.Application.Models.MultiTenancy
{
    public class BasicTenantInfo
    {
        public string Identifier { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public BasicTenantInfo() { }
     
    }
}