using System.Collections.Generic;
using MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAddresses;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IAddressRepository : IRepositoryAsync<Address, int>
    {
        //TODO: Add interface definitions
        public Task<Address> FindByStreetAddressLine1AndPostalCode(string addressLine1, string postalCode, int clientId);
        public Task<List<Address>> FindByCriteria(GetAddressesByCriteriaQuery criteria);
        public Task<Address> GetAddressByIdAsync(int addressId);
        public Task<List<Address>> GetListAsync();
    }
}