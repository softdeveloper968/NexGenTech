using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetAllTenantBasicIntoQuery : IRequest<Result<List<BasicTenantInfoResponse>>>
    {
    }

    public class GetAllTenantBasicIntoQueryHandler : IRequestHandler<GetAllTenantBasicIntoQuery, Result<List<BasicTenantInfoResponse>>>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;

        public GetAllTenantBasicIntoQueryHandler(IAdminUnitOfWork adminUnitOfWork)
        {
            _adminUnitOfWork = adminUnitOfWork;
        }
        public async Task<Result<List<BasicTenantInfoResponse>>> Handle(GetAllTenantBasicIntoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _adminUnitOfWork.Repository<Domain.IdentityEntities.Tenant, int>()
                                  .Entities
                                  .Where(x => x.IsActive)
                                  .Select(x => new BasicTenantInfoResponse
                                  {
                                      TenantId = x.Id,
                                      TenantName = x.TenantName,
                                      TenantIdentifier = x.Identifier
                                  })
                                  .ToListAsync();

                return Result<List<BasicTenantInfoResponse>>.Success(data);
            }
            catch (Exception ex)
            {
                return Result<List<BasicTenantInfoResponse>>.Fail("Failed to get tenant data.");
            }
        }
    }
}
