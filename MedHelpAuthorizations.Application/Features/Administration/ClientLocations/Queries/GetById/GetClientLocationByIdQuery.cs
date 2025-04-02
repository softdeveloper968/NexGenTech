using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Queries.GetById
{
    public class GetClientLocationByIdQuery : IRequest<Result<GetClientLocationByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetClientLocationByIdQueryHandler : IRequestHandler<GetClientLocationByIdQuery, Result<GetClientLocationByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientLocationByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetClientLocationByIdResponse>> Handle(GetClientLocationByIdQuery query, CancellationToken cancellationToken)
        {
            var clientLocation = await _unitOfWork.Repository<ClientLocation>().GetByIdAsync(query.Id);
            var mappedClientLocation = _mapper.Map<GetClientLocationByIdResponse>(clientLocation);
            return await Result<GetClientLocationByIdResponse>.SuccessAsync(mappedClientLocation);
        }
    }
}
