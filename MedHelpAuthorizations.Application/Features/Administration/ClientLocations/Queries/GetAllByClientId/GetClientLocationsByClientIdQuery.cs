using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged
{
    public class GetClientLocationsByClientIdQuery : IRequest<Result<List<GetClientLocationsByClientIdResponse>>>
    {
        public GetClientLocationsByClientIdQuery()
        {
        }
    }

    public class GetClientLocationsByClientIdQueryHandler : IRequestHandler<GetClientLocationsByClientIdQuery, Result<List<GetClientLocationsByClientIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public GetClientLocationsByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetClientLocationsByClientIdResponse>>> Handle(GetClientLocationsByClientIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientLocation, GetClientLocationsByClientIdResponse>> expression = e => _mapper.Map<GetClientLocationsByClientIdResponse>(e);
            var clientLocationCriteriaSpec = new ClientLocationsByClientIdSpecification(_clientId);

            var data = await _unitOfWork.Repository<ClientLocation>().Entities
               .Specify(clientLocationCriteriaSpec)
               .Select(expression)
               .ToListAsync();

            return await Result<List<GetClientLocationsByClientIdResponse>>.SuccessAsync(data);
        }
    }
}
