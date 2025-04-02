using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Queries.GetAllPagedData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientNoteManager : IClientNoteManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public ClientNoteManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<PaginatedResult<GetAllPagedClientNotesResponse>> GetClientNoteAsync(GetAllPagedClientNoteRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientNoteEndPoint.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToPaginatedResult<GetAllPagedClientNotesResponse>();
        }
        public async Task<IResult<int>> SaveAsync(AddEditClientNoteCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientNoteEndPoint.Save, request);
            return await response.ToResult<int>();
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientNoteEndPoint.Delete}/{id}");
            return await response.ToResult<int>();
        }
    }
}
