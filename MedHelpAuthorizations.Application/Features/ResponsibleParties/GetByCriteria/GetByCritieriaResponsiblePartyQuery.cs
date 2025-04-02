using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.ResponsibleParties.GetByCriteria
{
    public class GetByCritieriaResponsiblePartyQuery : IRequest<PaginatedResult<GetByCritieriaResponsiblePartyResponse>>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }


        public string PatientFirstName { get; set; }
        public string PatientLastName { get; set; }
        public string ResponsiblePartyFirstName { get; set; }
        public string ResponsiblePartyLastName { get; set; }
        public string ResponsiblePartyAccountNumber { get; set; }
        public int? PatientAccountNumber { get; set; }
        public string ExternalId { get; set; }
        public DateTime? DOB { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class GetResponsiblePartiesByCriteriaQueryHandler : IRequestHandler<GetByCritieriaResponsiblePartyQuery, PaginatedResult<GetByCritieriaResponsiblePartyResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        public GetResponsiblePartiesByCriteriaQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ;
            _mapper = mapper;
        }      

        public Task<PaginatedResult<GetByCritieriaResponsiblePartyResponse>> Handle(GetByCritieriaResponsiblePartyQuery request, CancellationToken cancellationToken)
        {
            var data = _unitOfWork.Repository<ResponsibleParty>()
                .Entities
                .Specify(new GetByCritieriaResponsiblePartyQuerySpec(request))
                .Select(x => _mapper.Map<GetByCritieriaResponsiblePartyResponse>(request))
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return data;
        }
    }
}
