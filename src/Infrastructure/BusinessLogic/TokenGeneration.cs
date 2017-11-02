using System.Collections.Generic;
using System.Linq;
using Infrastructure.ClientEntities;
using Infrastructure.DataContext;
using Infrastructure.Entities;

namespace Infrastructure.BusinessLogic
{
    public class TokenGeneration
    {
        //Not Used
        public IEnumerable<UnitToken> GetGeneratedTokens(IEnumerable<RegisterUnitToken> tokens, DatabaseContext context)
        {
            return tokens.Select(value => new UnitToken
            {
                Token = value?.Token ?? "",
                SiteName = context?.Sites?.FirstOrDefault(x => x.SiteId == value.SiteId)?.Name ?? "",
                Creator = value?.CreatorUserName ?? "",
                Name = value?.Name ?? "",
                UserId = value?.UserId,
                UnitNo = context?.Units?.FirstOrDefault(x => x.Id == value.UnitId).UnitNo ?? 0
            }).ToList();
        }
    }
}