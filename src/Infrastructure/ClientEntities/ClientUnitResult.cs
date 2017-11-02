using System;

namespace Infrastructure.ClientEntities
{
    public class ClientUnitResult
    {
        public Guid? AccountId { get; set; }
        public Guid? SiteId { get; set; }
        public Guid? ResultId { get; set; }
        public int? UnitId { get; set; }
        public string Error { get; set; }
    
    }
}