using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using System.Threading;

namespace MedHelpAuthorizations.Application.Multitenancy;

public class UpgradeSubscriptionRequest : IRequest<int>
{
    public int TenantId { get; set; } = default!;
    public DateTime ExtendedExpiryDate { get; set; }
}

public class UpgradeSubscriptionRequestValidator : CustomValidator<UpgradeSubscriptionRequest>
{
    public UpgradeSubscriptionRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}

public class UpgradeSubscriptionRequestHandler : IRequestHandler<UpgradeSubscriptionRequest, int>
{
    private readonly ITenantManagementService _tenantManagementService;

    public UpgradeSubscriptionRequestHandler(ITenantManagementService tenantManagementService) => _tenantManagementService = tenantManagementService;

    public Task<int> Handle(UpgradeSubscriptionRequest request, CancellationToken cancellationToken)
    {
        //_tenantManagementService.UpdateSubscription(request.TenantId, request.ExtendedExpiryDate);
        return null;
    }
}