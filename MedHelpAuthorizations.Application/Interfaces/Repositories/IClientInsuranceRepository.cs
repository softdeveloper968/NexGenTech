using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Features.Administration.ClientInsurances.Queries.GetByCriteriaPaged;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using System.Linq;
using MedHelpAuthorizations.Domain.Entities;

namespace MedHelpAuthorizations.Application.Interfaces.Repositories
{
    public interface IClientInsuranceRepository
    {
        IQueryable<ClientInsurance> ClientInsurances { get; } //EN-91
        Task<PaginatedResult<GetByCriteriaPagedInsurancesResponse>> GetByCriteria(GetByCriteriaPagedInsurancesQuery request);
    }
}

