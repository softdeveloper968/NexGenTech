using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Providers.Queries.GetProviderById
{
    public class GetProviderByIdQuery : IRequest<Result<GetProviderByIdResponse>>
    {
        public int Id { get; set; }
    }
    public class GetProviderByIdQueryHandler : IRequestHandler<GetProviderByIdQuery, Result<GetProviderByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProviderByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProviderByIdResponse>> Handle(GetProviderByIdQuery request, CancellationToken cancellationToken)
        {
            var prv = await _unitOfWork.Repository<ClientProvider>().GetByIdAsync(request.Id);
            var data = _mapper.Map<GetProviderByIdResponse>(prv);

            return await Result<GetProviderByIdResponse>.SuccessAsync(data);
        }
    }
}
