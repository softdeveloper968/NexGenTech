using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetAllByClientId
{
    public class GetAllCptCodeByClientIdQuery : IRequest<Result<List<GetClientCptCodeByIdResponse>>>
    {
        public GetAllCptCodeByClientIdQuery() { }
    }

    public class GetClientCptCodeByIdQueryHandler : IRequestHandler<GetAllCptCodeByClientIdQuery, Result<List<GetClientCptCodeByIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetClientCptCodeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetClientCptCodeByIdResponse>>> Handle(GetAllCptCodeByClientIdQuery query, CancellationToken cancellationToken)
        {
            Expression<Func<ClientCptCode, GetClientCptCodeByIdResponse>> expression = e => _mapper.Map<GetClientCptCodeByIdResponse>(e);
            var clientLocationCriteriaSpec = new CptCodeByClientIdSpecification(_clientId);
            var data = await _unitOfWork.Repository<ClientCptCode>().Entities
                .Specify(clientLocationCriteriaSpec).Select(expression).ToListAsync();
            return await Result<List<GetClientCptCodeByIdResponse>>.SuccessAsync(data);
        }
    }
}
