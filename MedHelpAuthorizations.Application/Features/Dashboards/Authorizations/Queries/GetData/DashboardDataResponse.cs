using System.Collections.Generic;
using MedHelpAuthorizations.Shared.Wrapper;

namespace MedHelpAuthorizations.Application.Features.Dashboards.Authorizations.Queries.GetData
{
    public class DashboardDataResponse
    {
        public List<ChartSeries> DataEnterBarChart { get; set; } = new List<ChartSeries>();
        public List<DashboardDataQueryResult> DashboardDataQueryResults { get; set; } = new List<DashboardDataQueryResult>();
    }

    public class ChartSeries
    {
        public ChartSeries() { }

        public string Name { get; set; }
        public double[] Data { get; set; }
    }
}