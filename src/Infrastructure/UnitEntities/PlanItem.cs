using System;
using System.Collections.Generic;

namespace Infrastructure.UnitEntities
{
    public class PlanItem
    {
        public int UnitNo { get; set; }
        public Guid AccountId { get; set; }
        public Guid SiteId { get; set; }
        public IEnumerable<UnitPlan> Payload { get; set; }
    }
}
