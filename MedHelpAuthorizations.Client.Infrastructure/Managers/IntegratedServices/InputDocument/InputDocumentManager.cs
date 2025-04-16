using HttpClientToCurl;
using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Queries.GetAll;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes.IntegratedServices;
using MedHelpAuthorizations.Shared.Requests.IntegratedServices.InputDocuments;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.InputDocument
{
    public class InputDocumentManager : IInputDocumentManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public InputDocumentManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{InputDocumentsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<GetAllInputDocumentsResponse>> GetAllAsync(GetAllPagedInputDocumentsRequest request)
        {
            var response = await _tenantHttpClient.GetAsync(InputDocumentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllInputDocumentsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditInputDocumentCommand request)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(InputDocumentsEndpoints.Save, request);
                var curlScript = _tenantHttpClient.Client.GenerateCurlInString(response.RequestMessage);

                return await response.ToResult<int>();
            }
            catch (System.Exception ex)
            {

                throw;
            }

        }

        public async Task<IResult<int>> SaveImportInputDocumentAsync(AddEditImportProcessInputDocumentCommand request)
        {
            try
            {
                var response = await _tenantHttpClient.PostAsJsonAsync(InputDocumentsEndpoints.SaveImportDocument, request);
                return await response.ToResult<int>();
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

		public async Task<IResult<int>> RetryImportInputDocumentAsync(AddEditImportProcessInputDocumentCommand request) //EN-317
		{
			try
			{
				var response = await _tenantHttpClient.PostAsJsonAsync(InputDocumentsEndpoints.RetryImportDocument, request);
				return await response.ToResult<int>();
			}
			catch (System.Exception ex)
			{
				throw;
			}
		}


        //public async Task<PaginatedResult<GetByCriteriaDocumentsResponse>> GetByCriteriaAsync(GetByCriteriaInputDocumentsQuery request)
        //{
        //    var response = await _tenantHttpClient.GetAsync(Routes.InputDocumentsEndpoints.GetByCriteriaPaged());
        //    return await response.ToPaginatedResult<GetByCriteriaDocumentsResponse>();
        //}


       

    }
}