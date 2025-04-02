using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.DenialReasons
{
    public interface IDenialReasonsManager : IManager
    {
        Task<IResult<List<ClaimSummary>>> GetDenialsByProcedureQueryAsync(GetDenialsByProcedureQuery criteria); //EN-289
        Task<IResult<List<ClaimSummary>>> GetDenialsByInsuranceQueryAsync(GetDenialReasonsByInsuranceQuery criteria); //EN-289
    }
}
