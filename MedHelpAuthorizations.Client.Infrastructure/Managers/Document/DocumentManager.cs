using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEditByPatient;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Requests.Documents;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Document
{
    public class DocumentManager : IDocumentManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public DocumentManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{Routes.DocumentsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.DocumentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllDocumentsResponse>();
        }

        public async Task<IResult<GetByIdDocumentsResponse>> GetByIdAsync(GetByIdDocumentsQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.DocumentsEndpoints.GetById(request.Id));
            return await response.ToResult<GetByIdDocumentsResponse>();
        }

        public async Task<PaginatedResult<GetByCriteriaDocumentsResponse>> GetByCriteriaAsync(GetByCriteriaDocumentsQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.DocumentsEndpoints.GetByCriteriaPaged(request.PatientId, request.AuthorizationId ,request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetByCriteriaDocumentsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDocumentByPatientCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.DocumentsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}