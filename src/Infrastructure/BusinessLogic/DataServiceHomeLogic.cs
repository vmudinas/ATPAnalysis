using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;

namespace Infrastructure.BusinessLogic
{
    public class DataServiceHomeLogic
    {
        public List<FailsByLocation> ReturnFailsChartLogic(List<MongoResult> results)
        {
            var query = from record in results
                group record by new
                {
                    record.LocationName
                }
                into g
                select new FailsByLocation
                {
                    Location = g.Key.LocationName,
                    NumberOfFails = g.Count(x => x.RLU > x.Upper)
                };

            return query.OrderByDescending(x=>x.NumberOfFails).Where(x => x.NumberOfFails != 0).ToList();
        }
    }
}