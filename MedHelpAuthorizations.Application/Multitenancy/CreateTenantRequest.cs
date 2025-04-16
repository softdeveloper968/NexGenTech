using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

namespace MedHelpAuthorizations.Application.Multitenancy;

public class CreateTenantRequest : IRequest<string>
{
    public int Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = default!;
    public string? Issuer { get; set; }
}

//public class CreateTenantRequestHandler : IRequestHandler<CreateTenantRequest, string>
//{
//    private readonly ITenantManagementService _tenantManagementService;

//    public CreateTenantRequestHandler(ITenantManagementService tenantManagementService) => _tenantManagementService = tenantManagementService;

//    public Task<string> Handle(CreateTenantRequest request, CancellationToken cancellationToken) =>
//       _tenantManagementService.CreateAsync(request, cancellationToken);        
//}