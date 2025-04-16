using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using System.Threading;

namespace MedHelpAuthorizations.Application.Multitenancy;

public class ActivateTenantRequest : IRequest<int>
{
    public int TenantId { get; set; } = default!;

    public ActivateTenantRequest(int tenantId) => TenantId = tenantId;
}

public class ActivateTenantRequestValidator : CustomValidator<ActivateTenantRequest>
{
    public ActivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}

public class ActivateTenantRequestHandler : IRequestHandler<ActivateTenantRequest, int>
{
    private readonly ITenantManagementService _tenantManagementService;

    public ActivateTenantRequestHandler(ITenantManagementService tenantManagementService) => _tenantManagementService = tenantManagementService;

    public Task<int> Handle(ActivateTenantRequest request, CancellationToken cancellationToken) 
    {
        //_tenantManagementService.ActivateAsync(request.TenantId);
        return null;
    }
}