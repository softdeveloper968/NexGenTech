using MedHelpAuthorizations.Application.Features.Admin.AuthType.Models;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Interfaces.Services.MultiTenancy;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.AuthType.Queries
{
    public class GetAllAuthTypeQuery : IRequest<Result<IEnumerable<GetAllAuthTypeResponse>>>
    {
        public int TenantId { get; set; }
        public GetAllAuthTypeQuery(int tenantId)
        {
            TenantId = tenantId;
        }
    }

    public class GetAllAuthTypeQueryHandler : IRequestHandler<GetAllAuthTypeQuery, Result<IEnumerable<GetAllAuthTypeResponse>>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly ITenantManagementService _tenantManagementService;

        public GetAllAuthTypeQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory, ITenantManagementService tenantManagementService)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _tenantManagementService = tenantManagementService;
        }
        public async Task<Result<IEnumerable<GetAllAuthTypeResponse>>> Handle(GetAllAuthTypeQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Domain.Entities.AuthType, GetAllAuthTypeResponse>> expression = e => new GetAllAuthTypeResponse
            {
                Id = e.Id,
                Name = e.Name,
            };

            string tenantIdentifier = (await _tenantManagementService.GetByIdAsync(request.TenantId)).Identifier;

            var unitOnWork = _tenantRepositoryFactory.GetUnitOfWork<int>(tenantIdentifier);

            var data = await unitOnWork.Repository<Domain.Entities.AuthType>().Entities
               .Select(expression)
               .ToListAsync();

            return Result<IEnumerable<GetAllAuthTypeResponse>>.Success(data);
        }
    }
}
