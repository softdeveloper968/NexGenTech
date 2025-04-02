using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Documents.Commands.AddEditByPatient;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Documents.Queries.GetByCriteria;
using MedHelpAuthorizations.Application.Requests.Documents;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<int>> SaveAsync(AddEditDocumentByPatientCommand request);

        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<GetByIdDocumentsResponse>> GetByIdAsync(GetByIdDocumentsQuery request);
        Task<PaginatedResult<GetByCriteriaDocumentsResponse>> GetByCriteriaAsync(GetByCriteriaDocumentsQuery request);
    }
}