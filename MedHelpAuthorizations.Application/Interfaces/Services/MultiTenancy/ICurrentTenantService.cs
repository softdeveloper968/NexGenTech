
namespace MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy
{
    public interface ICurrentTenantService
    {
        public string GetDatabaseProvider();
        public string GetConnectionString();
        public string GetTenantIdentifier();    
        public string GetTenantName();
    }
}
