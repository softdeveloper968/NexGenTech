using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Persons.Queries.GetPersonsByCriteria;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class PersonRepositoryAsync : RepositoryAsync<Person, int>, IPersonRepository
    {
        private readonly ApplicationContext _dbContext;

        public PersonRepositoryAsync(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Person> Persons => _dbContext.Persons;

		public async Task<Person> GetFirstPersonByCriteriaAsync(string firstName, string lastName, DateTime? dateOfBirth, string email, int clientId)
		{
            //Require first and last name. And one of: (DOB, AddressLine1, phoneNumber, email)
			if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || (dateOfBirth == null && string.IsNullOrWhiteSpace(email)))
			{
				return null;
			}

            var query = new GetPersonsByCriteriaQuery() { FirstName = firstName, LastName = lastName, DateOfBirth = dateOfBirth, Email = email };

			return await Persons
                        .Specify<Person>(new PersonByCriteriaSpecification(query, clientId))
                        .FirstOrDefaultAsync();
		}

		public async Task<Person> GetPersonByIdAsync(int personId)
        {
            return await Persons
                           .Include(x => x.Address)
                           .ThenInclude(y => y.State)
                           .Where(p => p.Id == personId)
                           .FirstOrDefaultAsync();
        }
	}
}
