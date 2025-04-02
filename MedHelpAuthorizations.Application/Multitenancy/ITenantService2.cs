using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Multitenancy;

//public interface ITenantService2
//{
//    Task<List<TenantDto>> GetAllAsync();
//    Task<bool> ExistsWithIdAsync(string id);
//    Task<bool> ExistsWithNameAsync(string name);
//    Task<TenantDto> GetByIdAsync(string id);
//    Task<string> CreateAsync(CreateTenantRequest request, CancellationToken cancellationToken);
//    Task<string> ActivateAsync(string id);
//    Task<string> DeactivateAsync(string id);
//    Task<string> UpdateSubscription(string id, DateTime extendedExpiryDate);
//}