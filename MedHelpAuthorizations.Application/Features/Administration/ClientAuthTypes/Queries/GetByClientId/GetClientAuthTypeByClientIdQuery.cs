using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetByClientId
{
	public class GetClientAuthTypesByClientIdQuery : IRequest<Result<List<GetClientAuthTypesByClientIdResponse>>>
    {

    }

    public class GetClientByIdQueryHandler : IRequestHandler<GetClientAuthTypesByClientIdQuery, Result<List<GetClientAuthTypesByClientIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetClientByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetClientAuthTypesByClientIdResponse>>> Handle(GetClientAuthTypesByClientIdQuery query, CancellationToken cancellationToken)
        {            
            var data = await _unitOfWork.Repository<ClientAuthType>()
                .Entities
                .Specify(new ClientAuthTypesByClientIdSpecification(_currentUserService.ClientId))
                .Select(x => _mapper.Map<GetClientAuthTypesByClientIdResponse>(x))
                .ToListAsync(); 
            
            return await Result<List<GetClientAuthTypesByClientIdResponse>>.SuccessAsync(data);
        }
    }
}
