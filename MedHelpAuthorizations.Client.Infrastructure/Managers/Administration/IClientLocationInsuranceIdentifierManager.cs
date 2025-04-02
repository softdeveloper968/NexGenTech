using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Queries.GetByLocationId;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientLocationInsuranceIdentifierManager : IManager
    {
        Task<IResult<int>> DeleteAsync(int id);
        Task<IResult<int>> SaveAsync(AddEditClientLocationInsuranceIdentifierCommand command);
        Task<IResult<List<GetClientLocationInsuranceIdentifierByLocationIdResponse>>> GetAllByClientLocationIdAsync(int locatiopnId);
    }
}