using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Commands
{
    public class AddEditTenantCommand : IRequest<IResult<int>>
    {
        public int TenantId { get; set; }
        public int DatabaseServerId { get; set; }
        public string TenantIdentifier { get; set; }
        public string TenantName { get; set; }
        public string DatabaseName { get; set; }
        public string AdminEmail { get; set; }
        public bool IsActive { get; set; }
        public DateTime ValidUpto { get; set; }
        public bool IsProductionTenant { get; set; }
    }

    public class AddEditTenantCommandHandler : IRequestHandler<AddEditTenantCommand, IResult<int>>
    {
        private readonly ITenantManagementService _tenantManagementService;
        private readonly ITenantMediatorService _tenantMediatorService;

        public AddEditTenantCommandHandler(ITenantManagementService tenantManagementService, ITenantMediatorService tenantMediatorService)
        {
            _tenantManagementService = tenantManagementService;
            _tenantMediatorService = tenantMediatorService;
        }
        public async Task<IResult<int>> Handle(AddEditTenantCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.TenantId == 0)
                {
                    int id = await _tenantManagementService.CreateAsync(request.DatabaseServerId, request.TenantName, request.TenantIdentifier, request.DatabaseName, request.IsActive, null, request.AdminEmail, request.ValidUpto, request.IsProductionTenant);
                    var tenant = await _tenantManagementService.GetEntityByIdAsync(id);
                    await _tenantMediatorService.InitializeTenant(tenant, cancellationToken);
                    return Result<int>.Success(id);
                }
                else
                {
                    await _tenantManagementService.UpdateAsync(request.TenantId, request.DatabaseServerId, request.TenantName, request.TenantIdentifier, request.DatabaseName, request.IsActive, null, request.AdminEmail, request.ValidUpto, request.IsProductionTenant);
                    return Result<int>.Success(request.TenantId);
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Fail(string.Format("Failed to {0} tenant", request.TenantId == 0 ? "create" : "update"));
            }
        }
    }
}
