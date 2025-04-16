using MedHelpAuthorizations.Domain.Entities;
using System.Linq;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IPersonRepository : IRepositoryAsync<Person, int>
    {
        IQueryable<Person> Persons { get; }
        Task<Person> GetPersonByIdAsync(int personId);
		//Task<List<Person>> GetPersonsByCriteriaAsync(string firstName, string lastName, DateTime? dateOfBirth, string addressLine1, long? phoneNumber, string email);
        Task<Person> GetFirstPersonByCriteriaAsync(string firstName, string lastName, DateTime? dateOfBirth, string email, int clientId);
	}
}