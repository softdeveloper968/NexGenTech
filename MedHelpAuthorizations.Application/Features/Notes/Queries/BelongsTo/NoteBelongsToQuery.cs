using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
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

namespace MedHelpAuthorizations.Application.Features.Notes.Queries.BelongsTo
{
  public class NoteBelongsToQuery : IRequest<Result<NoteBelongsToResponse>>
  {
    public int Id { get; set; }

  }

  public class NoteBelongsToQueryHandler : IRequestHandler<NoteBelongsToQuery, Result<NoteBelongsToResponse>>
  {
    private readonly IUnitOfWork<int> _unitOfWork;
    private readonly IMapper _mapper;

    public NoteBelongsToQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
    }

    public async Task<Result<NoteBelongsToResponse>> Handle(NoteBelongsToQuery query, CancellationToken cancellationToken)
    {
      var note = await _unitOfWork.Repository<Note>()
        .Entities
        .Include(x => x.Authorization)
        .Select(x => new NoteBelongsToResponse()
        { 
          Id = x.Id,
          AuthorizationId = x.AuthorizationId,
          PatientId = x.Authorization.PatientId
        })
        .FirstOrDefaultAsync(x => x.Id == query.Id);     
      return await Result<NoteBelongsToResponse>.SuccessAsync(note);
    }
  }
}
