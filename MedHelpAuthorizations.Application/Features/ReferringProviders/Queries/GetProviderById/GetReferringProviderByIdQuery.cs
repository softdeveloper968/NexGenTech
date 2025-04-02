using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ReferringProviders.Queries.GetReferringProviderById
{
    public class GetReferringProviderByIdQuery : IRequest<Result<GetReferringProviderByIdResponse>>
    {
        public int Id { get; set; }
    }
    public class GetReferringProviderByIdQueryHandler : IRequestHandler<GetReferringProviderByIdQuery, Result<GetReferringProviderByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetReferringProviderByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetReferringProviderByIdResponse>> Handle(GetReferringProviderByIdQuery request, CancellationToken cancellationToken)
        {
            var prv = await _unitOfWork.Repository<ReferringProvider>().GetByIdAsync(request.Id);
            var data = _mapper.Map<GetReferringProviderByIdResponse>(prv);

            return await Result<GetReferringProviderByIdResponse>.SuccessAsync(data);
        }
    }
}
