using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetTenantNameById: IRequest<Result<string>>
    {
        public GetTenantNameById(int tenantId)
        {
            TenantId = tenantId;
        }

        public int TenantId { get; }
    }
    public class GetTenantNameByIdHandler : IRequestHandler<GetTenantNameById, Result<string>>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        public GetTenantNameByIdHandler(IAdminUnitOfWork adminUnitOfWork)
        {
            _adminUnitOfWork = adminUnitOfWork;
        }
        public async Task<Result<string>> Handle(GetTenantNameById request, CancellationToken cancellationToken)
        {
            try
            {
                string data = await _adminUnitOfWork.Repository<Domain.IdentityEntities.Tenant, int>()
                                  .Entities
                                  .Where(x => x.IsActive && x.Id == request.TenantId)
                                  .Select(x => x.TenantName)
                                  .FirstAsync();

                return Result<string>.Success(data,"");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail("Failed to get tenant data.");
            }
        }
    }
}
