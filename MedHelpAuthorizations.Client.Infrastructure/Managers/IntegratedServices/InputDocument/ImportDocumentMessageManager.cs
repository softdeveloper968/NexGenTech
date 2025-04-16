using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetInputDocumentMessageById;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Requests.IntegratedServices.ImportDocumentMessage;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.InputDocument
{
	public class ImportDocumentMessageManager : IImportDocumentMessageManager
	{
        private readonly ITenantHttpClient _tenantHttpClient;

        public ImportDocumentMessageManager(ITenantHttpClient tenantHttpClient)
		{
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<List<ImportDocumentMessageResponseModel>>> GetImportMessageByInputDocumentIdAsync(int inputDocumenId)
		{
			var response = await _tenantHttpClient.GetAsync(ImportDocumentMessageEndPoints.GetByInputDcocumentId(inputDocumenId));
			return await response.ToResult<List<ImportDocumentMessageResponseModel>>();
		}

		public async Task<PaginatedResult<ImportDocumentMessageResponse>> GetAllAsync(GetAllPagedImportDocumentMessageRequest request)
		{
			var response = await _tenantHttpClient.GetAsync(ImportDocumentMessageEndPoints.GetAllPaged(request.PageNumber, request.PageSize, request.InputDocumentId, request.MessageType));
			return await response.ToPaginatedResult<ImportDocumentMessageResponse>();
		}

		public async Task<string> GetExportData(int inputDocumentId, string messageType)
		{
			var response = await _tenantHttpClient.GetAsync(ImportDocumentMessageEndPoints.GetExportdata(inputDocumentId, messageType));
			var data = await response.Content.ReadAsStringAsync();
			return data;
		}
	}
}
