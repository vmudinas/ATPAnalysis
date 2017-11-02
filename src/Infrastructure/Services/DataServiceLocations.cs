using System.Collections.Generic;
using System.Linq;
using Infrastructure.Entities;
using Infrastructure.Services.Abstraction;

namespace Infrastructure.Services
{
    public partial class DataService : IDataService
    {
        #region Locations

        public IEnumerable<Location> GetLocations(string userName)
        {
            // Filter By Sites
            //TODO
            return _repo.Get<Location>(x => x.AccountId == GetDefaultAccountId(userName)).ToList();
        }

        #endregion
    }
}