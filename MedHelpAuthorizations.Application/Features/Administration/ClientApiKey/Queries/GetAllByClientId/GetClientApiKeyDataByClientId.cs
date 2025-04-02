using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientApiKey.Queries.GetAllByClientId
{
    public class GetClientApiKeyDataByClientId : IRequest<Result<List<ApiKeyViewModel>>>
    {
        public int ClientId { get; set; }
    }

    public class GetClientApiKeyDataByClientIdHandler : IRequestHandler<GetClientApiKeyDataByClientId, Result<List<ApiKeyViewModel>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientApiKeyDataByClientIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ApiKeyViewModel>>> Handle(GetClientApiKeyDataByClientId request, CancellationToken cancellationToken)
        {

            Expression<Func<ClientApiIntegrationKey, ApiKeyViewModel>> expression = e => _mapper.Map<ApiKeyViewModel>(e);
            var clientApiKeyCriteriaSpec = new ClientApiKeyByClientIdSpecification(request.ClientId);

            var data = await _unitOfWork.Repository<ClientApiIntegrationKey>().Entities
               .Specify(clientApiKeyCriteriaSpec)
               .Select(expression)
               .ToListAsync();

            return await Result<List<ApiKeyViewModel>>.SuccessAsync(data);
        }
    }
}
