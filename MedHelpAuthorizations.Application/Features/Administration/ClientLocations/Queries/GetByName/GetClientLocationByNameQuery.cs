using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetByName
{
    public class GetClientLocationByNameQuery : IRequest<Result<GetClientLocationByNameResponse>>
    {
        public string LocationName { get; set; }
        public int ClientId { get; set; } = 1;
    }

    public class GetClientLocationByNameQueryHandler : IRequestHandler<GetClientLocationByNameQuery, Result<GetClientLocationByNameResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientLocationByNameQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetClientLocationByNameResponse>> Handle(GetClientLocationByNameQuery query, CancellationToken cancellationToken)
        {
            ///Todo: confirm from kevin, do we need to check for clientId with locationName.
            var clientLocation = await _unitOfWork.Repository<ClientLocation>()
                                                  .Entities
                                                  .FirstOrDefaultAsync(x => x.Name == query.LocationName && x.ClientId == query.ClientId);
            var mappedClientLocation = _mapper.Map<GetClientLocationByNameResponse>(clientLocation);
            return await Result<GetClientLocationByNameResponse>.SuccessAsync(mappedClientLocation);
        }
    }
}
