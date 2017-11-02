namespace Infrastructure.ClientEntities
{
    public class UnitToken
    {
        public string Token { get; set; }
        public string SiteName { get; set; }
        public string Creator { get; set; }
        public int UnitNo { get; set; }
        public string Name { get; set; }

        public string UserId { get; set; }

        public bool IsCloudSync { get; set; }
    }
}