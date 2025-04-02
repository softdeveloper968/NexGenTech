using AutoMapper;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId
{
    public class GetClaimStatusWorkstationNotesByIdQuery : IRequest<Result<GetNotesByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetClaimStatusWorkstationNotesByIdHandler : IRequestHandler<GetClaimStatusWorkstationNotesByIdQuery, Result<GetNotesByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetClaimStatusWorkstationNotesByIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<GetNotesByIdResponse>> Handle(GetClaimStatusWorkstationNotesByIdQuery query, CancellationToken cancellationToken)
        {
            var note = await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().GetByIdAsync(query.Id);
            var data = _mapper.Map<GetNotesByIdResponse>(note);
            return await Result<GetNotesByIdResponse>.SuccessAsync(data);
        }
    }
}
