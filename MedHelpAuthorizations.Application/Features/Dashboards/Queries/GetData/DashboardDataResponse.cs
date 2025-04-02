using MedHelpAuthorizations.Shared.Wrapper;
using System.Collections.Generic;

namespace MedHelpAuthorizations.Application.Features.Dashboards.Queries.GetData
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