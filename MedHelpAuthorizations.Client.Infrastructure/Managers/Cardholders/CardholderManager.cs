using MedHelpAuthorizations.Application.Features.Cardholders.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.CardholderViewModels;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Cardholders
{
    public class CardholderManager : ICardholderManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public CardholderManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }
        
        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync(Routes.CardholderEndpoints.Delete(id));
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<CardholderViewModel>> GetByCriteriaAsync(GetCardholdersByCriteriaQuery request)
        {           
            var response = await _tenantHttpClient.GetAsync(Routes.CardholderEndpoints.GetByCriteria(request));
            return await response.ToPaginatedResult<CardholderViewModel>();
        }
        
        public async Task<IResult<CardholderViewModel>> GetByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.CardholderEndpoints.GetById(id));
            return await response.ToResult<CardholderViewModel>();
        }

        public async Task<IResult<List<CardholderViewModel>>> GetBySearchStringAsync(string searchString)
        {
            var response = await _tenantHttpClient.GetAsync(Routes.CardholderEndpoints.GetBySearchString(searchString));
            return await response.ToResult<List<CardholderViewModel>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditCardholderCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(Routes.CardholderEndpoints.Save(), request);
            return await response.ToResult<int>();
        }
    }
}
