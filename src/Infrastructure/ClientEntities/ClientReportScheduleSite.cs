using System;

namespace Infrastructure.ClientEntities
{
    public class ClientReportScheduleSite
    {
        public virtual Guid Id { get; set; }

        public virtual Guid SiteId { get; set; }

        public virtual string Name { get; set; }

        public virtual Guid ScheduleId { get; set; }
    }
}