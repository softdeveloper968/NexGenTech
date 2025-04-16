using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using System.Threading;

namespace MedHelpAuthorizations.Application.Multitenancy;

public class GetTenantRequest : IRequest<TenantDto>
{
    public int TenantId { get; set; } = default!;

    public GetTenantRequest(int tenantId) => TenantId = tenantId;
}

public class GetTenantRequestValidator : CustomValidator<GetTenantRequest>
{
    public GetTenantRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}

public class GetTenantRequestHandler : IRequestHandler<GetTenantRequest, TenantDto>
{
    private readonly ITenantManagementService _tenantManagementService;

    public GetTenantRequestHandler(ITenantManagementService tenantManagementService) => _tenantManagementService = tenantManagementService;

    public Task<TenantDto> Handle(GetTenantRequest request, CancellationToken cancellationToken) =>
        _tenantManagementService.GetByIdAsync(request.TenantId);
}