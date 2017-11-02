using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;

namespace Infrastructure.BusinessLogic
{
    public class FailsChartLogic
    {
        public List<FailsChart> ReturnFailsChartLogic(List<MongoResult> results)
        {
            var fails = new FailsChart
            {
                Color = "red",
                Area = results.Count(x => x.RLU > x.Upper),
                Result = "Fail"
            };
            var passes = new FailsChart
            {
                Color = "green",
                Area = results.Count(x => x.RLU <= x.Lower),
                Result = "Pass"
            };
            var cautions = new FailsChart
            {
                Color = "orange",
                Area = results.Count(x => x.RLU <= x.Upper && x.RLU > x.Lower),
                Result = "Caution"
            };

            return new List<FailsChart> {fails, passes, cautions};
        }
    }
}