using System;

namespace Infrastructure.UnitEntities
{
    public class RegisterUnitConfirmation
    {
        public Guid? SiteId { get; set; }
        public Guid? AccountId { get; set; }
        public string UnitUserName { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
        public string UnitName { get; set; }
    }
}