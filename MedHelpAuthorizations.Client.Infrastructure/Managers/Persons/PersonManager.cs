using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Application.Features.Persons.Queries.GetAllPersons;
using MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsByCriteria;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MedHelpAuthorizations.Client.Infrastructure.Extensions;
using MedHelpAuthorizations.Client.Infrastructure.HttpClients;
using MedHelpAuthorizations.Client.Infrastructure.Routes;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Persons
{
    public class PersonManager : IPersonManager
    {
        private readonly ITenantHttpClient _tenantHttpClient;

        public PersonManager(ITenantHttpClient tenantHttpClient)
        {
            _tenantHttpClient = tenantHttpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _tenantHttpClient.DeleteAsync($"{PersonEndoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<string> ExportToExcelAsync()
        {
            var response = await _tenantHttpClient.GetAsync(PersonEndoints.Export);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }

        public async Task<IResult<string>> GetPersonImageAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(PersonEndoints.GetPersonImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<PersonDto>> GetAllPagedPersonsAsync(GetAllPagedPersonsQuery request)
        {
            var response = await _tenantHttpClient.GetAsync(PersonEndoints.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<PersonDto>();
        }

        public async Task<IResult<int>> SaveAsync(UpsertPersonCommand request)
        {
            var response = await _tenantHttpClient.PostAsJsonAsync(PersonEndoints.Save, request);
            return await response.ToResult<int>();
        }

        public async Task<PaginatedResult<PersonDto>> GetByCriteriaAsync(GetPersonsByCriteriaQuery query)
        {
            var response = await _tenantHttpClient.GetAsync(PersonEndoints.GetByCriteriaPaged(query));
            return await response.ToPaginatedResult<PersonDto>();
        }

        public async Task<IResult<PersonDto>> GetPersonByIdAsync(int id)
        {
            var response = await _tenantHttpClient.GetAsync(PersonEndoints.GetById(id));
            return await response.ToResult<PersonDto>();
        }        
    }
}