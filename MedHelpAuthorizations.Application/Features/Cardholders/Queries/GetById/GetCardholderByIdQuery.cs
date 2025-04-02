using AutoMapper;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.CardholderViewModels;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetById
{
    public class GetCardholderByIdQuery : IRequest<Result<CardholderViewModel>>
    {
        public int Id { get; set; }
    }

    public class GetCardholderByIdQueryHandler : IRequestHandler<GetCardholderByIdQuery, Result<CardholderViewModel>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCardholderByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CardholderViewModel>> Handle(GetCardholderByIdQuery query, CancellationToken cancellationToken)
        {
            var cardholder = await _unitOfWork.Repository<Cardholder>().GetByIdAsync(query.Id);
            var mappedCardholder = _mapper.Map<CardholderViewModel>(cardholder);
            return await Result<CardholderViewModel>.SuccessAsync(mappedCardholder);
        }
    }
}
