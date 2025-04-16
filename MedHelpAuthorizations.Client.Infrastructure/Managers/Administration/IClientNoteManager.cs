using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Queries.GetAllPagedData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientNoteManager : IManager
    {
        Task<PaginatedResult<GetAllPagedClientNotesResponse>> GetClientNoteAsync(GetAllPagedClientNoteRequest request);
        Task<IResult<int>> SaveAsync(AddEditClientNoteCommand request);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
