using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
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

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId
{
    public class GetNotesByAuthorizationIdQuery : IRequest<Result<List<GetNotesByAuthorizationIdResponse>>>
    {
        public int AuthorizationId { get; set; }
    }

    public class GetNotesByAuthorizationIdHandler : IRequestHandler<GetNotesByAuthorizationIdQuery, Result<List<GetNotesByAuthorizationIdResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUserClientRepository _userClientRepository;
        private readonly IUserService _userService;

        //private readonly IMapper _mapper;

        public GetNotesByAuthorizationIdHandler(IUnitOfWork<int> unitOfWork, IUserClientRepository userClientRepository, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            //_mapper = mapper;
            _userClientRepository = userClientRepository;
            _userService = userService;
        }
        public async Task<Result<List<GetNotesByAuthorizationIdResponse>>> Handle(GetNotesByAuthorizationIdQuery query, CancellationToken cancellationToken)
        {
            var notes = await _unitOfWork.Repository<Note>()
                .Entities
                .Specify(new NotesByAuthorizationIdSpecification(query.AuthorizationId))
                .Select(x => new GetNotesByAuthorizationIdResponse()
                {
                    Id = x.Id,
                    AuthorizationId = x.AuthorizationId,
                    NoteUserId = x.NoteUserId,
                    CreatedBy = _userService.GetNameAsync(x.CreatedBy).Result,
                    CreatedOn = x.CreatedOn,
                    ClientId = x.ClientId,
                    NoteContent = x.NoteContent
                })
                .ToListAsync();            
            return await Result<List<GetNotesByAuthorizationIdResponse>>.SuccessAsync(notes);
        }
    }
}
