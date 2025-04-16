using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientDocuments.Queries;
using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Command.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Queries.GetAllPagedData;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public class ClientDocumentManager : IClientDocumentManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;
        public ClientDocumentManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<PaginatedResult<GetAllPagedClientDocumentsResponse>> GetClientDocumentsAsync(GetAllPagedClientDocumentRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(ClientDocumentEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString));
            return await response.ToPaginatedResult<GetAllPagedClientDocumentsResponse>();
        }
        public async Task<IResult<int>> SaveAsync(AddEditClientDocumentCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(ClientDocumentEndpoints.Save, request);
            return await response.ToResult<int>();
        }
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{ClientDocumentEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }
    }
}
