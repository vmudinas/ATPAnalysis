using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.BusinessLogic;
using Infrastructure.ClientEntities;
using Infrastructure.MongoEntities;
using Infrastructure.Services.Abstraction;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        #region Home Screen 

        public IEnumerable<MongoResult> GetAllHomeData(DateTime fromUtc, DateTime toUtc, ClientAccount account)
        {
            if (account == null || fromUtc.Equals(toUtc)) return null;

            var siteList = account.Sites.Select(s => s.SiteId.ToString()).ToList();

            

            var results = _repo.GetMongo<MongoResult>("Results",
               x => fromUtc <= x.ResultDate && x.ResultDate <= toUtc
                           && x.AccountId == account.AccountId.ToString()
                           && siteList.Contains(x.SiteId), null,  "{ ResultDate: 0}",0,  15000);

            return results;

        
        }

      
        #endregion
    }
}