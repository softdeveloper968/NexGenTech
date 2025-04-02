using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.AuthTypes.Queries.GetById;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IAuthTypeManager : IManager
    {
        Task<PaginatedResult<GetAllPagedAuthTypesResponse>> GetAuthTypesPaginatedAsync(GetAllPagedAuthTypesRequest request);
        Task<IResult<List<GetAllPagedAuthTypesResponse>>> GetAuthTypesAsync(); //AA-23
        Task<IResult<int>> SaveAsync(AddEditAuthTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<string> ExportToExcelAsync();
        Task<IResult<GetAuthTypeByIdResponse>> GetAuthTypesByIdAsync(GetAuthTypeByIdQuery request);
    }
}