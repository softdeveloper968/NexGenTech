using MedHelpAuthorizations.Application.Features.Dashboards.IntegratedServices.Queries.GetData;
using MedHelpAuthorizations.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Client.Infrastructure.Managers.IntegratedServices.Comparison
{
    public interface IComparisonDashboardManager : IManager
    {
        Task<IResult<List<ProviderComparisonResponse>>> GetProviderDetails(ProviderComparisonQuery query);
    }
}
