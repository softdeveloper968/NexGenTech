using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes
{
    public class GetClientLocationServiceTypesQuery : IRequest<Result<List<GetClientLocationServiceTypesResponse>>>
    {
        public int LocationId { get; set; }
    }

    public class GetClientLocationServiceTypesQueryHandler : IRequestHandler<GetClientLocationServiceTypesQuery, Result<List<GetClientLocationServiceTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetClientLocationServiceTypesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetClientLocationServiceTypesResponse>>> Handle(GetClientLocationServiceTypesQuery query, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<ClientLocationTypeOfService>()
                .Entities
                .Specify(new ClientLocationServiceTypesByClientIdSpecification(query, _currentUserService.ClientId))
                .Select(x => _mapper.Map<GetClientLocationServiceTypesResponse>(x))
                .ToListAsync();

            return await Result<List<GetClientLocationServiceTypesResponse>>.SuccessAsync(data);
        }
    }
}
