using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Multitenancy;

public class GetAllTenantsRequest : IRequest<List<TenantDto>>
{
}

public class GetAllTenantsRequestHandler : IRequestHandler<GetAllTenantsRequest, List<TenantDto>>
{
    private readonly ITenantManagementService _tenantManagementService;

    public GetAllTenantsRequestHandler(ITenantManagementService tenantManagementService) => _tenantManagementService = tenantManagementService;

    public Task<List<TenantDto>> Handle(GetAllTenantsRequest request, CancellationToken cancellationToken) =>
        _tenantManagementService.GetAllAsync();
}