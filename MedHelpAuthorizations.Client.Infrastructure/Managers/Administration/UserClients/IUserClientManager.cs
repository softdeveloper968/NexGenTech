using MedHelpAuthorizations.Application.Features.UserClients.Commands.AddEdit;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.UserClients.Queries.GetByUserId;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration.UserClients
{
    public interface IUserClientManager : IManager
    {
        Task<PaginatedResult<GetByUserIdQueryResponse>> GetClientsByUserAsync(GetByUserIdQuery request);
        Task<IResult<string>> SaveAsync(AddEditUserClientCommand request);
    }
}