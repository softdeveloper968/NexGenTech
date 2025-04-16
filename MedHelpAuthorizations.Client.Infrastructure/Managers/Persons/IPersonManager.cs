using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Persons.Commands.UpsertPerson;
using MedHelpAuthorizations.Application.Features.Persons.Queries.GetAllPersons;
using MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsByCriteria;
using MedHelpAuthorizations.Application.Features.Persons.ViewModels;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Persons
{
    public interface IPersonManager : IManager
    {
        Task<PaginatedResult<PersonDto>> GetAllPagedPersonsAsync(GetAllPagedPersonsQuery request);

        Task<IResult<string>> GetPersonImageAsync(int id);
        Task<IResult<PersonDto>> GetPersonByIdAsync(int id);

        Task<IResult<int>> SaveAsync(UpsertPersonCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<string> ExportToExcelAsync();
        
        Task<PaginatedResult<PersonDto>> GetByCriteriaAsync(GetPersonsByCriteriaQuery query);
    }
}