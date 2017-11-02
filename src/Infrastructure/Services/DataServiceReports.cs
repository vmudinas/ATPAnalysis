using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.BusinessLogic;
using Infrastructure.ClientEntities;
using Infrastructure.Entities;
using Infrastructure.MongoEntities;
using Infrastructure.Services.Abstraction;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        #region Pass Caution Fail

        public IEnumerable<ReportPieChart> GetReports(DateTime fromDate, DateTime toUtc, string userName,
            string reportType)
        {
            return
                new ReportPieChartLogic().ReturnFailsChartLogic(
                    _repo.GetMongo<MongoResult>("Results",x =>
                        fromDate <= x.ResultDate && x.ResultDate <= toUtc &&
                        x.AccountId == GetDefaultAccountId(userName).ToString()).ToList()
                      )
                        .ToList();
            ;
        }

        #endregion
    }
}