using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ResponsibleParties.GetAllPaged
{
    public class GetAllPagedResponsiblePartyQuery : IRequest<PaginatedResult<GetAllPagedResponsiblePartyResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }


    public class GetAllPagedResponsiblePartyQueryHandler: IRequestHandler<GetAllPagedResponsiblePartyQuery, PaginatedResult<GetAllPagedResponsiblePartyResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
       
        public GetAllPagedResponsiblePartyQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllPagedResponsiblePartyResponse>> Handle(GetAllPagedResponsiblePartyQuery request, CancellationToken cancellationToken)
        {
            var rp = await _unitOfWork.Repository<ResponsibleParty>()
                .Entities
                .Select(x => _mapper.Map<GetAllPagedResponsiblePartyResponse>(x))
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return rp;            
        }
    }


}
