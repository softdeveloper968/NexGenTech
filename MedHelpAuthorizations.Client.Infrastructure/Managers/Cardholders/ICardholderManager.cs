using MedHelpAuthorizations.Application.Features.Cardholders.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.CardholderViewModels;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Cardholders
{
    public interface ICardholderManager : IManager
    {
        Task<PaginatedResult<CardholderViewModel>> GetByCriteriaAsync(GetCardholdersByCriteriaQuery request);
        Task<IResult<List<CardholderViewModel>>> GetBySearchStringAsync(string searchtext);
        Task<IResult<CardholderViewModel>> GetByIdAsync(int id);
        Task<IResult<int>> SaveAsync(AddEditCardholderCommand request);
        Task<IResult<int>> DeleteAsync(int id);
    }
}
