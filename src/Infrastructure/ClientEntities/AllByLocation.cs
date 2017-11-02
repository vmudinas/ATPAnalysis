using System;

namespace Infrastructure.ClientEntities
{
    public class AllByLocation
    {
        public DateTime ResultDate { get; set; }
        public int? RLU { get; set; }
        //public int? Pass { get; set; }
        //public int? Caution { get; set; }
        public int? Lower { get; set; }
        public int? Upper { get; set; }
    }
}