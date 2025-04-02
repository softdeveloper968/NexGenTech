using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.GetById
{
    public class GetNotesByIdQuery : IRequest<Result<GetNotesByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetNotesByIdHandler : IRequestHandler<GetNotesByIdQuery, Result<GetNotesByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetNotesByIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<GetNotesByIdResponse>> Handle(GetNotesByIdQuery query, CancellationToken cancellationToken)
        {
            var note = await _unitOfWork.Repository<Note>().GetByIdAsync(query.Id);
            var data = _mapper.Map<GetNotesByIdResponse>(note);
            return await Result<GetNotesByIdResponse>.SuccessAsync(data);
        }
    }
}
