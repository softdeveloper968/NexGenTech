using MedHelpAuthorizations.Application.Features.Administration.Clients.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetAllPaged;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetById;
using MedHelpAuthorizations.Application.Features.Administration.Clients.Queries.GetClientsByCriteria;
using MedHelpAuthorizations.Application.Requests.Administration;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientManager : IManager
    {
        Task<PaginatedResult<GetAllPagedClientsResponse>> GetClientsAsync(GetAllPagedClientsRequest request);

        Task<IResult<string>> GetClientImageAsync(int id);
        Task<IResult<GetClientByIdResponse>> GetClientByIdAsync(int id = 0);

        Task<IResult<int>> SaveAsync(AddEditClientCommand request);

        Task<IResult<int>> DeleteAsync(int id);
        Task<string> ExportToExcelAsync();
        Task<PaginatedResult<GetClientsByCriteriaResponse>> GetByCriteriaAsync(GetClientsByCriteriaQuery query);
        Task<IResult<List<MedHelpAuthorizations.Domain.Entities.Client>>> GetAllClient();
        //Task<IResult<List<TenantResponse>>> GetTenants(); //AA-206
    }
}
