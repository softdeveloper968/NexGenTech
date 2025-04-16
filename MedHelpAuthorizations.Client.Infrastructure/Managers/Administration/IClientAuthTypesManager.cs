using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetByClientId;
using MedHelpAuthorizations.Application.Features.Administration.ClientAuthTypes.Queries.GetClientLocationServiceTypes;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientAuthTypesManager : IManager
    {
        Task<IResult<List<GetClientAuthTypesByClientIdResponse>>> GetByClientIdAsync();
        Task<IResult<List<GetClientLocationServiceTypesResponse>>> GetClientLocationServiceTypes(int locationId);
    }
}