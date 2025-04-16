using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Commands.AddEdit;
using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetBillingKpi;
using MedHelpAuthorizations.Application.Models.IntegratedServices;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.Administration
{
    public interface IClientKpiManager : IManager
    {
        Task<IResult<int>> SaveAsync(AddEditClientKpisCommand request);
        Task<IResult<ClientKpiDto>> GetClientkpiByClientIdAsync(int clientId);

        /// <summary>
        /// To Get billing kpi data
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<IResult<GetBillingKpiByClientIdResponse>> GetBillingKpiByClientIdAsync(GetBillingKpiByClientIdQuery query);
    }
}
