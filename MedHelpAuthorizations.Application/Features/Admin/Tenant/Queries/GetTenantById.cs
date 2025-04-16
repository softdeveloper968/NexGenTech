using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetTenantById : IRequest<IResult<TenantInfoResponse>>
    {
        public int TenantId { get; set; }
        public GetTenantById(int tenantId)
        {
            TenantId = tenantId;
        }
    }

    public class GetTenantByIdHandler : IRequestHandler<GetTenantById, IResult<TenantInfoResponse>>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        private readonly IUserService _userService;

        public GetTenantByIdHandler(IAdminUnitOfWork adminUnitOfWork, IUserService userService)
        {
            _adminUnitOfWork = adminUnitOfWork;
            _userService = userService;
        }
        public async Task<IResult<TenantInfoResponse>> Handle(GetTenantById request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.IdentityEntities.Tenant, TenantInfoResponse>> expression = e => new TenantInfoResponse
            {
                TenantId = e.Id,
                TenantIdentifier = e.Identifier,
                TenantName = e.TenantName,
                AdminEmail = e.AdminEmail,
                DatabaseName = e.DatabaseName,
                IsActive = e.IsActive,
                ServerId = e.DatabaseServer.Id,
                ServerName = e.DatabaseServer.ServerName,
                ValidUpto = e.ValidUpto,
                CreatedByName = _userService.GetNameAsync(e.CreatedBy).Result,
                CreatedOn = e.CreatedOn,
                LastModifiedByName = string.IsNullOrEmpty(e.LastModifiedBy) ? "" : _userService.GetNameAsync(e.LastModifiedBy).Result,
                LastModifiedOn = e.LastModifiedOn,
                IsProductionTenant = e.IsProductionTenant,
            };

            try
            {
                var data = await _adminUnitOfWork.Repository<Domain.IdentityEntities.Tenant, int>().Entities
                    .Where(x=>x.Id == request.TenantId)
                    .Select(expression)
                    .FirstOrDefaultAsync();

                if (data == null)
                {
                    return Result<TenantInfoResponse>.Fail("Tenant not found");
                }

                return Result<TenantInfoResponse>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<TenantInfoResponse>.Fail("Something went wrong.");
            }
        }
    }
}
