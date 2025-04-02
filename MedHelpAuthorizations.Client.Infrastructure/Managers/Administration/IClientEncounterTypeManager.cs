using MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Queries.GetAllPagedData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientEncounterTypeManager : IManager
    {
        Task<PaginatedResult<GetAllPagedClientEncounterTypesResponse>> GetEncounterTypesAsync(GetAllPagedEncounterTypeRequest request);
        Task<IResult<int>> SaveAsync(AddEditClientEncounterTypeCommand request);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
