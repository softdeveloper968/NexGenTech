using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Queries.GetAll;
using MedHelpAuthorizations.Shared.Requests.IntegratedServices.InputDocuments;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.InputDocument
{
    public interface IInputDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllInputDocumentsResponse>> GetAllAsync(GetAllPagedInputDocumentsRequest request);

        Task<IResult<int>> SaveAsync(AddEditInputDocumentCommand request);
        Task<IResult<int>> SaveImportInputDocumentAsync(AddEditImportProcessInputDocumentCommand request);

		Task<IResult<int>> RetryImportInputDocumentAsync(AddEditImportProcessInputDocumentCommand request);

        //Task<IResult<int>> DeleteAsync(int id);
        //Task<IResult<GetByIdInputDocumentsResponse>> GetByIdAsync(GetByIdInputDocumentsQuery request);
        //Task<PaginatedResult<GetByCriteriaInputDocumentsResponse>> GetByCriteriaAsync(GetByCriteriaInputDocumentsQuery request);

    }
}