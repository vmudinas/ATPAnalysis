using System;
using System.Collections.Generic;

namespace Infrastructure.ClientEntities
{
    // a collection of this class is consumed by the report engine and email sending sections
    public class ReportSchedulePayload
    {
        // using ReportId instead of ReportName because (especially for custom reports) various 
        // related items (like filters, etc.) including report name need to be queried anyway.
        public virtual Guid ReportId { get; set; }
        public virtual byte ReportPeriod { get; set; }
        public virtual string EmailList { get; set; }
        public virtual IEnumerable<ClientReportScheduleSite> SiteIds { get; set; }

        // Normally, a scheduled report will use ReportPeriod to construct start 
        // and end dates. However, if we resend reports that had failed in past, 
        // we will need actual start and end dates for report date filters.
        public virtual string StartDate { get; set; }
        public virtual string EndDate { get; set; }
        /// <summary>
        /// this is the order in which the payload should attempt to be sent.
        /// it's important for updating ReportSchedule.LastSent to preserve business logic.
        /// </summary>
        public virtual DateTime DateStamp { get; set; }
        public virtual Guid ScheduleId { get; set; }
        public virtual string ScheduleName { get; set; } // this is the "appointment" title placed on dxScheduler
    }
}