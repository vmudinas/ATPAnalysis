using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;

namespace Infrastructure.BusinessLogic
{
    public class ReportPieChartLogic
    {
        public List<ReportPieChart> ReturnFailsChartLogic(List<MongoResult> results)
        {
            var fails = new ReportPieChart
            {
                Color = "red",
                Value = results.Count(x => x.RLU > x.Upper),
                Label = "Fail"
            };
            var passes = new ReportPieChart
            {
                Color = "green",
                Value = results.Count(x => x.RLU <= x.Lower),
                Label = "Pass"
            };
            var cautions = new ReportPieChart
            {
                Color = "orange",
                Value = results.Count(x => x.RLU <= x.Upper && x.RLU > x.Lower),
                Label = "Caution"
            };

            return new List<ReportPieChart> {fails, passes, cautions};
        }
    }
}