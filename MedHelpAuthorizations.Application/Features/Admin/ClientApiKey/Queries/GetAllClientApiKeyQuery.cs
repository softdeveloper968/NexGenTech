using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Admin.ClientApiKey.Queries
{
    public class GetAllClientApiKeyQuery : IRequest<PaginatedResult<ApiKeyViewModel>>
    {
        public int TenantId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public int ClientId { get; set; }

        public GetAllClientApiKeyQuery(int tenantId, int pageNumber, int pageSize, string searchString, int clientId)
        {
            TenantId = tenantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            ClientId = clientId;
        }
    }

    public class GetAllClientApiKeyQueryHandler : IRequestHandler<GetAllClientApiKeyQuery, PaginatedResult<ApiKeyViewModel>>
    {
        private readonly ITenantRepositoryFactory _tenantRepositoryFactory;
        private readonly IMapper _mapper;

        public GetAllClientApiKeyQueryHandler(ITenantRepositoryFactory tenantRepositoryFactory, IMapper mapper)
        {
            _tenantRepositoryFactory = tenantRepositoryFactory;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<ApiKeyViewModel>> Handle(GetAllClientApiKeyQuery request, CancellationToken cancellationToken)
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
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}
