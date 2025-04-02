using MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.EncounterTypes.Queries.GetAllPagedData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientEncounterTypeManager : IClientEncounterTypeManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public ClientEncounterTypeManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<PaginatedResult<GetAllPagedClientEncounterTypesResponse>> GetEncounterTypesAsync(GetAllPagedEncounterTypeRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientEncounterTypeEndPoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedClientEncounterTypesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditClientEncounterTypeCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientEncounterTypeEndPoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientEncounterTypeEndPoints.Delete}/{id}");
            return await response.ToResult<int>();
        }
    }
}
