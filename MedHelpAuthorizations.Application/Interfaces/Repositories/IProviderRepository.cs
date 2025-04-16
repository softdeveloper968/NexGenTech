using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IProviderRepository : IRepositoryAsync<ClientProvider, int>
    {
         Task<ClientProvider> FindByNpiAsync(string npi);
        //Task<IEnumerable<Provider>> FindBySpecialty(int specialtyId);
        //Task<IEnumerable<Provider>> FindBySpecialty(Specialty specialty);
        //Task<IEnumerable<GetAllProvidersResponse>> FindByCriteria(GetProvidersByCriteriaParameterRequest criteria);
    }
}
