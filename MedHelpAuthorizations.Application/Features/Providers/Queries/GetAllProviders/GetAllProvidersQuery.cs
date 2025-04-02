using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Providers.Queries.GetAllProviders
{
    namespace MedHelpAuthorizations.Application.Features.Providers.GetAllProviders
    {
        public class GetAllProvidersQuery : IRequest<Result<List<GetAllProvidersResponse>>>
        {

        }
        public class GetAllProvidersQueryHandler : IRequestHandler<GetAllProvidersQuery, Result<List<GetAllProvidersResponse>>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;
            private int _clientId => _currentUserService.ClientId;

            public GetAllProvidersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
            {
                _unitOfWork = unitOfWork ;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }

            public async Task<Result<List<GetAllProvidersResponse>>> Handle(GetAllProvidersQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var data = await _unitOfWork.Repository<ClientProvider>()
                                   .Entities
                                   .Include(x => x.Person)
                                   .ThenInclude(x => x.Address)
                                   .Specify(new GenericByClientIdSpecification<ClientProvider>(_clientId))
                                   .Select(x => _mapper.Map<GetAllProvidersResponse>(x))
                                   .ToListAsync();

                    return await Result<List<GetAllProvidersResponse>>.SuccessAsync(data);
                }
                catch (Exception ex)
                {
                    return await Result<List<GetAllProvidersResponse>>.FailAsync($"Error returning Client Providers: {ex.Message}");
                }             
            }
        }
    }
}
