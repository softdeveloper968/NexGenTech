using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Queries.GetByLocationId
{
    public class GetClientLocationInsuranceIdentifierByLocationIdQuery : IRequest<Result<List<GetClientLocationInsuranceIdentifierByLocationIdResponse>>>
    {
        public int LocationId { get; set; }

        public GetClientLocationInsuranceIdentifierByLocationIdQuery(int locationId)
        {
            LocationId = locationId;
        }
    }

    public class GetClientLocationInsuranceIdentifierByLocationIdQueryHandler : IRequestHandler<GetClientLocationInsuranceIdentifierByLocationIdQuery, Result<List<GetClientLocationInsuranceIdentifierByLocationIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public GetClientLocationInsuranceIdentifierByLocationIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService; 
        }

        public async Task<Result<List<GetClientLocationInsuranceIdentifierByLocationIdResponse>>> Handle(GetClientLocationInsuranceIdentifierByLocationIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Expression<Func<Domain.Entities.ClientLocationInsuranceIdentifier, GetClientLocationInsuranceIdentifierByLocationIdResponse>> expression = e => _mapper.Map<GetClientLocationInsuranceIdentifierByLocationIdResponse>(e);

                var data = await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>().Entities
                    .Include(x => x.ClientInsurance)
                   .Specify(new GenericByClientIdSpecification<Domain.Entities.ClientLocationInsuranceIdentifier>(_clientId))
                   .Where(x => x.ClientLocationId == request.LocationId)
                   .Select(expression)
                   .ToListAsync();

                return await Result<List<GetClientLocationInsuranceIdentifierByLocationIdResponse>>.SuccessAsync(data);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
