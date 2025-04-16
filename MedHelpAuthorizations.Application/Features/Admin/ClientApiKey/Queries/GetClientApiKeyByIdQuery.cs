using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Admin.Client.Models;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Services.Admin;
using MedHelpAuthorizations.Application.Specifications;
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

namespace MedHelpAuthorizations.Application.Features.Admin.ClientApiKey.Queries
{
    public class GetClientApiKeyByIdQuery : IRequest<IResult<ApiKeyViewModel>>
    {
        public int TenantId { get; set; }
        public int ClientId { get; set; }
        public int ClientApiIntegrationId { get; }
        public GetClientApiKeyByIdQuery(int tenantId, int clientId, int clientApiIntegrationId)
        {
            TenantId = tenantId;
            ClientId = clientId;
            ClientApiIntegrationId = clientApiIntegrationId;
        }
    }
    public class GetClientApiKeyByIdQueryHandler : IRequestHandler<GetClientApiKeyByIdQuery, IResult<ApiKeyViewModel>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;

        public GetClientApiKeyByIdQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory, ITenantManagementService tenantManagementService, IMapper mapper)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
        }
        public async Task<IResult<ApiKeyViewModel>> Handle(GetClientApiKeyByIdQuery request, CancellationToken cancellationToken)
        {
            var unitOfWork = _tenantRepositoryFactory.GetUnitOfWork<int>(request.TenantId);

            Expression<Func<ClientApiIntegrationKey, ApiKeyViewModel>> expression = e => new ApiKeyViewModel
            {
                Id = e.Id,
                ClientId = e.ClientId,
                ApiKey = e.ApiKey,
                ApiIntegrationId = (int)e.ApiIntegrationId,
                ApiUrl = e.ApiUrl
            };

            var clientApiKeyCriteriaSpec = new ClientApiKeyByClientIdSpecification(request.ClientId);

            var data = await unitOfWork.Repository<ClientApiIntegrationKey>().Entities
               .Specify(clientApiKeyCriteriaSpec)
               .Select(expression)
               .Where(x => x.Id == request.ClientApiIntegrationId && x.ClientId == request.ClientId)
               .FirstAsync();

            return Result<ApiKeyViewModel>.Success(data);
        }
    }
}
