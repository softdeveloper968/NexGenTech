using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetAllPaged;
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
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetLocationDataByClientId
{
    public class GetLocationDataByClientIdQuery : IRequest<Result<List<GetClientLocationsByClientIdResponse>>>
    {
        public int ClientId { get; set; }
        public GetLocationDataByClientIdQuery()
        {
        }
    }

    public class GetLocationDataByClientIdQueryHandler : IRequestHandler<GetLocationDataByClientIdQuery, Result<List<GetClientLocationsByClientIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetLocationDataByClientIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetClientLocationsByClientIdResponse>>> Handle(GetLocationDataByClientIdQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientLocation, GetClientLocationsByClientIdResponse>> expression = e => _mapper.Map<GetClientLocationsByClientIdResponse>(e);
            var clientLocationCriteriaSpec = new ClientLocationsByClientIdSpecification(request.ClientId);

            var data = await _unitOfWork.Repository<ClientLocation>().Entities
               .Specify(clientLocationCriteriaSpec)
               .Select(expression)
               .ToListAsync();

            return await Result<List<GetClientLocationsByClientIdResponse>>.SuccessAsync(data);
        }
    }
}
