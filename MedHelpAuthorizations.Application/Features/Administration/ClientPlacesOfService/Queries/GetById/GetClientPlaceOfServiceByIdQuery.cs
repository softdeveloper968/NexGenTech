using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientPlacesOfService.Queries.GetById
{
    public class GetClientPlaceOfServiceByIdQuery : IRequest<Result<GetClientPlaceOfServiceByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetClientPlaceOfServiceByIdQueryHandler : IRequestHandler<GetClientPlaceOfServiceByIdQuery, Result<GetClientPlaceOfServiceByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClientPlaceOfServiceByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetClientPlaceOfServiceByIdResponse>> Handle(GetClientPlaceOfServiceByIdQuery query, CancellationToken cancellationToken)
        {
            var clientPlaceOfService = await _unitOfWork.Repository<Domain.Entities.ClientPlaceOfService>().GetByIdAsync(query.Id);
            var mappedClientPlaceOfService = _mapper.Map<GetClientPlaceOfServiceByIdResponse>(clientPlaceOfService);
            return await Result<GetClientPlaceOfServiceByIdResponse>.SuccessAsync(mappedClientPlaceOfService);
        }
    }
}
