using System;
using System.Collections.Generic;

namespace Infrastructure.ClientEntities
{
    public class ClientCloudSyncResult
    {
        public Guid AccountId { get; set; }
        public Guid SiteId { get; set; }
        public List<CloudSyncResultItem> Payload { get; set; }
    }
}