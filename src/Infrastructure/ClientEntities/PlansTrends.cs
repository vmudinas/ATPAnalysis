namespace Infrastructure.ClientEntities
{
    public class PlansTrends
    {
        public string LocationName { get; set; }
        public int Fail { get; set; }
        public int Pass { get; set; }
        public int Caution { get; set; }
    }
}