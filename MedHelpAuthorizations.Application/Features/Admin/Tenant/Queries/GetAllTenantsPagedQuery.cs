using MedHelpAuthorizations.Application.Features.Admin.Tenant.Models;
using MedHelpAuthorizations.Application.Interfaces.Repositories.Admin;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Domain.IdentityEntities;
using System.Linq.Expressions;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Application.Features.Admin.Server.Common;
using MedHelpAuthorizations.Application.Extensions;

namespace MedHelpAuthorizations.Application.Features.Admin.Tenant.Queries
{
    public class GetAllTenantsPagedQuery : IRequest<PaginatedResult<TenantInfoResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public bool IsActiveOnly { get; set; }
        public string Search { get; set; }
        public GetAllTenantsPagedQuery(int pageNumber, int pageSize, string search, bool isActiveOnly)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            IsActiveOnly = isActiveOnly;
            Search = search;
        }
    }

    public class GetAllTenantsPagedQueryHandler : IRequestHandler<GetAllTenantsPagedQuery, PaginatedResult<TenantInfoResponse>>
    {
        private readonly IAdminUnitOfWork _adminUnitOfWork;
        private readonly IUserService _userService;
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetAllTenantsPagedQueryHandler(IAdminUnitOfWork adminUnitOfWork, IUserService userService, ITenantRepositoryFactory tenantRepositoryFactory)
        {
            _adminUnitOfWork = adminUnitOfWork;
            _userService = userService;
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<PaginatedResult<TenantInfoResponse>> Handle(GetAllTenantsPagedQuery request, CancellationToken cancellationToken)
        {
            try
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
                    LastModifiedOn = e.LastModifiedOn
                };

                var query = _adminUnitOfWork.Repository<Domain.IdentityEntities.Tenant, int>()
                          .Entities.Select(expression)
                          .Where(x =>
                          (string.IsNullOrEmpty(request.Search) ||
                          (
                            x.TenantIdentifier.Contains(request.Search) ||
                            x.TenantName.Contains(request.Search) ||
                            x.DatabaseName.Contains(request.Search) ||
                            x.AdminEmail.Contains(request.Search) ||
                            x.ServerName.Contains(request.Search)
                          )));


                if (request.IsActiveOnly)
                {
                    query = query.Where(x => x.IsActive);
                }

                var data = await query.ToPaginatedListAsync(request.PageNumber, request.PageSize);


                foreach (var tenant in data.Data)
                {
                    var tenantRepo = _tenantRepositoryFactory.GetClientInsuranceRpaConfigurationRepository(tenant.TenantIdentifier);
                    int failedCount = await tenantRepo.GetFailedClientInsuranceRpaConfigurationsCountAsync();
                    tenant.TenantFailedConfigurationCount = failedCount;
                }

                return data;

            }
            catch (Exception ex)
            {
                return PaginatedResult<TenantInfoResponse>.Failure("Something went wrong.");
            }
        }
    }
}
