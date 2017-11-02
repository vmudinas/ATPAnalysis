using System;

namespace Infrastructure.ClientEntities
{
    public class FailByPlan
    {
        public string Location { get; set; }
        public DateTime ResultDate { get; set; }
        public int RLU { get; set; }
        public int? Upper { get; set; }
        public int UpperCount { get; set; }
        public int? Lower { get; set; }
        public int LowerCount { get; set; }
        public int CautionCount { get; set; }
    }
}