using MedHelpAuthorizations.Application.Interfaces.Services;
using System.Threading;

namespace MedHelpAuthorizations.Application.Multitenancy;

public class DeactivateTenantRequest : IRequest<string>
{
    public int TenantId { get; set; } = default!;

    public DeactivateTenantRequest(int tenantId) => TenantId = tenantId;
}

public class DeactivateTenantRequestValidator : CustomValidator<DeactivateTenantRequest>
{
    public DeactivateTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}

//public class DeactivateTenantRequestHandler : IRequestHandler<DeactivateTenantRequest, string>
//{
//    private readonly ITenantManagementService _tenantManagementService;

//    public DeactivateTenantRequestHandler(ITenantManagementService tenantManagementService) => _tenantManagementService = tenantManagementService;

//    public Task<string> Handle(DeactivateTenantRequest request, CancellationToken cancellationToken) =>
//        //tenantManagementService.DeactivateAsync(request.TenantId);
//}