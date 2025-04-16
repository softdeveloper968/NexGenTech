using AutoMapper;
using MedHelpAuthorizations.Application.Features.ReferringProviders.GetByCriteria;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Extensions;
using Microsoft.EntityFrameworkCore;


namespace MedHelpAuthorizations.Application.Features.ReferringProviders.Queries.GetAllReferringProviders
{
    namespace MedHelpAuthorizations.Application.Features.ReferringProviders.GetAllReferringProviders
    {
        public class GetAllReferringProvidersQuery : IRequest<Result<List<GetAllReferringProvidersResponse>>>
        {

        }
        public class GetAllReferringProvidersQueryHandler : IRequestHandler<GetAllReferringProvidersQuery, Result<List<GetAllReferringProvidersResponse>>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ICurrentUserService _currentUserService;
            private int _clientId => _currentUserService.ClientId;

            public GetAllReferringProvidersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
            {
                _unitOfWork = unitOfWork ;
                _mapper = mapper;
                _currentUserService = currentUserService;
            }

            public async Task<Result<List<GetAllReferringProvidersResponse>>> Handle(GetAllReferringProvidersQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var data = await _unitOfWork.Repository<ReferringProvider>()
                                   .Entities
                                   .Include(x => x.Person)
                                   .ThenInclude(x => x.Address)
                                   .Specify(new GenericByClientIdSpecification<ReferringProvider>(_clientId))
                                   .Select(x => _mapper.Map<GetAllReferringProvidersResponse>(x))
                                   .ToListAsync();

                    return await Result<List<GetAllReferringProvidersResponse>>.SuccessAsync(data);
                }
                catch (Exception ex)
                {
                    return await Result<List<GetAllReferringProvidersResponse>>.FailAsync($"Error returning Client ReferringProviders: {ex.Message}");
                }             
            }
        }
    }
}
