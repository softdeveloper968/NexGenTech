using MedHelpAuthorizations.Application.Features.Notes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Notes.Queries.BelongsTo;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Notes.Queries.GetNotesByAuthorizationId;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Notes
{
    public interface INoteManager : IManager
    {
        Task<IResult<NoteBelongsToResponse>> BelongsTo(int id);
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<List<GetNotesByAuthorizationIdResponse>>> GetByAuthIdAsync(int aid);
        Task<IResult<GetNotesByIdResponse>> GetById(int id);
        Task<IResult<int>> SaveAsync(AddEditNotesCommand command);
    }
}