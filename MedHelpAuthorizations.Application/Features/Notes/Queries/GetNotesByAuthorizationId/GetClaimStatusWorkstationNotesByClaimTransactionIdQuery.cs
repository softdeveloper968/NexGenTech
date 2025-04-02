using AutoMapper;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
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
    public class GetClaimStatusWorkstationNotesByClaimTransactionIdQuery : IRequest<Result<List<GetClaimStatusWorkstationNotesResponse>>>
    {
        public int Id { get; set; }
    }

    public class GetClaimStatusWorkstationNotesByClaimTransactionIdHandler : IRequestHandler<GetClaimStatusWorkstationNotesByClaimTransactionIdQuery, Result<List<GetClaimStatusWorkstationNotesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        IUserClientRepository _userClientRepository;
        private readonly IUserService _userService;

        public GetClaimStatusWorkstationNotesByClaimTransactionIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserClientRepository userClientRepository, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userClientRepository = userClientRepository;
            _userService = userService;
        }
        public async Task<Result<List<GetClaimStatusWorkstationNotesResponse>>> Handle(GetClaimStatusWorkstationNotesByClaimTransactionIdQuery query, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<ClaimStatusWorkstationNotes>().Entities
                       .Where(x => x.ClaimStatusTransactionId == query.Id)
                       .Select(t => new GetClaimStatusWorkstationNotesResponse
                       {
                           Id = t.Id,
                           ClaimStatusTransactionId = t.ClaimStatusTransactionId,
                           ClientId = t.ClientId,
                           UserName= _userService.GetNameAsync(t.CreatedBy).Result,
                           NoteContent = t.NoteContent,
                           NoteTs = t.NoteTs
                       }).ToListAsync();

            return await Result<List<GetClaimStatusWorkstationNotesResponse>>.SuccessAsync(data);
        }
    }
}
