using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAddresses;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Repositories
{
    public class AddressRepositoryAsync : RepositoryAsync<Address, int>, IAddressRepository
    {
        private readonly IRepositoryAsync<Address, int> _repository;
        private readonly ApplicationContext _dbContext;
        private readonly IMapper _mapper;

        public AddressRepositoryAsync(ApplicationContext dbContext, IRepositoryAsync<Address, int> repository, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _repository = repository;
            _mapper = mapper;
        }
        public IQueryable<Address> Addresses => _repository.Entities;

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _dbContext.Addresses
                           .Include(y => y.State)
                           .Where(a => a.Id == addressId)
                           .FirstOrDefaultAsync();
        }
        public async Task<List<Address>> GetListAsync()
        {
            return await _dbContext.Addresses.ToListAsync();
        }
        public Task<List<Address>> FindByStreetAddress(string streetName, int streetNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Address>> FindByCriteria(GetAddressesByCriteriaQuery criteria)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Address> FindByStreetAddressLine1AndPostalCode(string addressLine1, string postalCode, int clientId)
        {
            return await _dbContext.Addresses.Include(y => y.State)
                                       .Specify(new GenericByClientIdSpecification<Address>(clientId))
                                       .Where(a => !string.IsNullOrWhiteSpace(a.AddressStreetLine1) && a.AddressStreetLine1.ToLower().Trim() == addressLine1.ToLower().Trim()
                                              && !string.IsNullOrWhiteSpace(a.PostalCode) && a.PostalCode.Trim() == postalCode.Trim())
                                       .FirstOrDefaultAsync();
        }
    }
}
