using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.IntegratedServices.ImportDocumentMessage.Queries.GetInputDocumentMessageById;
using MedHelpAuthorizations.Shared.Requests.IntegratedServices.ImportDocumentMessage;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.InputDocument
{
	public interface IImportDocumentMessageManager : IManager
	{
		Task<PaginatedResult<ImportDocumentMessageResponse>> GetAllAsync(GetAllPagedImportDocumentMessageRequest request);
		Task<IResult<List<ImportDocumentMessageResponseModel>>> GetImportMessageByInputDocumentIdAsync(int inputDocumenId);
		Task<string> GetExportData(int inputDocumentId, string messageType);
	}
}
