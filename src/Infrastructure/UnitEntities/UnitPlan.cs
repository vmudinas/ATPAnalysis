using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.UnitEntities
{
    public class UnitPlan
    {
        public virtual Guid PlanId { get; set; }
        public virtual string PlanName { get; set; }
        public virtual bool IsRandom { get; set; }
        public virtual int Quota { get; set; }
        public virtual bool ShouldPreventRepeat { get; set; }
        public virtual DateTime ModifiedDate { get; set; } // this UTC date is both created by and modified by on unit.
        public virtual string ModifiedBy { get; set; } // unit user unless not logged in (unit login not required), then "System"
        public virtual IEnumerable<Guid> Locations { get; set; } // list of LocationIds for this plan
        public virtual IEnumerable<Guid> RequiredLocations { get; set; } // list of required LocationIds for this plan
    }
}
