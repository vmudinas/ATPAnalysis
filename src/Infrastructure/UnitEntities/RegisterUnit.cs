namespace Infrastructure.UnitEntities
{
    public class RegisterUnit
    {
        public int UnitSerialNo { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }

        public bool IsCloudSync { get; set; } = false;
    }
}