namespace Infrastructure.ClientEntities
{
    public class MongoStatus
    {
        public long Inserted { get; set; }
        public long Updated { get; set; }
        public long Deleted { get; set; }
        public long Total { get; set; }
    }
}