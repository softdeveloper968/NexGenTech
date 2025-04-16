using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetAllClientsByTenants : IRequest<Result<List<BasicTenantClientResponse>>>
    {
        public List<int> TenantIds { get; set; }
        public GetAllClientsByTenants(List<int> tenantIds)
        {
            TenantIds = tenantIds;
        }
    }
    public class GetAllClientsByTenantsHandler : IRequestHandler<GetAllClientsByTenants, Result<List<BasicTenantClientResponse>>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IAdminUnitOfWork _adminUnitOfWork;

        public GetAllClientsByTenantsHandler(ITenantRepositoryFactory tenantRepositoryFactory, IAdminUnitOfWork adminUnitOfWork)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _adminUnitOfWork = adminUnitOfWork;
        }
        public async Task<Result<List<BasicTenantClientResponse>>> Handle(GetAllClientsByTenants request, CancellationToken cancellationToken)
        {
            try
            {
                var tenantIds = request.TenantIds;

                List<BasicTenantClientResponse> response = new List<BasicTenantClientResponse>();

                var tenants = await _adminUnitOfWork.Repository<Domain.IdentityEntities.Tenant, int>()
                  .Entities
                  .Where(x => tenantIds.Contains(x.Id))
                  .Select(x => new BasicTenantInfoResponse
                  {
                      TenantId = x.Id,
                      TenantName = x.TenantName
                  })
                  .ToListAsync();

                foreach (var tenantId in tenantIds)
                {
                    var clientRepository = _tenantRepositoryFactory.Get<IClientRepository>(tenantId);
                    var clients = await clientRepository.GetAllBasicClients();

                    if (clients.Count() > 0)
                    {
                        response.Add(new BasicTenantClientResponse()
                        {
                            TenantId = tenantId,
                            TenantName = tenants.First(x => x.TenantId == tenantId).TenantName,
                            Clients = clients.ToList(),
                        });
                    }
                }
                return Result<List<BasicTenantClientResponse>>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<List<BasicTenantClientResponse>>.Fail("Failed to load tenant client data");
            }
        }
    }
}
