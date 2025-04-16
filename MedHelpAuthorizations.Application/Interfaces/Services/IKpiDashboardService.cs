using MedHelpAuthorizations.Application.Features.Administration.ClientKpis.Queries.GetBillingKpi;
using MedHelpAuthorizations.Application.Interfaces.Common;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Interfaces.Services
{
    public interface IKpiDashboardService : IService
    {
        /// <summary>
        /// To get the billing KPI data by client Id
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<GetBillingKpiByClientIdResponse> GetBillingKpiByClientIdAsync(GetBillingKpiByClientIdQuery query);
    }
}
