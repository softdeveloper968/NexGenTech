using MedHelpAuthorizations.Application.Features.DocumentTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Commands.Delete;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.DocumentType
{
    public class DocumentTypeManager : IDocumentTypeManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public DocumentTypeManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        public async Task<PaginatedResult<GetAllDocumentTypeResponse>> GetAllPagedAsync(GetAllPagedDocumentTypeQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.DocumentTypeEndPoints.GetAllPaged);
            return await response.ToPaginatedResult<GetAllDocumentTypeResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.DocumentTypeEndPoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync(Routes.DocumentTypeEndPoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(Routes.DocumentTypeEndPoints.Export);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
