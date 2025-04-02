using MedHelpAuthorizations.Application.Features.DocumentTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Commands.Delete;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAll;
using MedHelpAuthorizations.Application.Features.DocumentTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.DocumentType
{
    public interface IDocumentTypeManager : IManager
    {
        Task<IResult<int>> DeleteAsync(int id);
        Task<PaginatedResult<GetAllDocumentTypeResponse>> GetAllPagedAsync(GetAllPagedDocumentTypeQuery request);
        Task<IResult<int>> SaveAsync(AddEditDocumentTypeCommand request);
        Task<string> ExportToExcelAsync();
    }
}