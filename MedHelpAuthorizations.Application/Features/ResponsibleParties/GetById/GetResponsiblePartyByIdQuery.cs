using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ResponsibleParties.GetById
{
    public class GetResponsiblePartyByIdQuery : IRequest<Result<GetResponsiblePartyByIdResponse>>
    {
        public int Id { get; set; }
    }


    public class GetResponsiblePartyByIdQueryHandler : IRequestHandler<GetResponsiblePartyByIdQuery, Result<GetResponsiblePartyByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
       
        public GetResponsiblePartyByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetResponsiblePartyByIdResponse>> Handle(GetResponsiblePartyByIdQuery request, CancellationToken cancellationToken)
        {
            var rp = await _unitOfWork.Repository<ResponsibleParty>().GetByIdAsync(request.Id);
            var data = _mapper.Map<GetResponsiblePartyByIdResponse>(rp);
            return await Result<GetResponsiblePartyByIdResponse>.SuccessAsync(data);

        }
    }
}
