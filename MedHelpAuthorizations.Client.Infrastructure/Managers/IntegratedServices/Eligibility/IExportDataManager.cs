using MedHelpAuthorizations.Shared.Wrapper;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetAllPaged;
using self_pay_eligibility_api.Application.Features.Eligibility.EligibilityCheckRequests.Queries.GetByCriteria;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.Eligibility
{
    public interface IExportDataManager : IManager
    {
        Task<IResult<List<GetEligibilityCheckDetailsByCriteriaResponse>>> GetDetailsByCriteriaAsync(GetEligibilityCheckDetailsByCriteriaQuery request);
    }
}

