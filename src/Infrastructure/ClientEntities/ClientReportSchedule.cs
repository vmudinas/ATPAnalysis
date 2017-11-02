using System;
using System.Collections.Generic;

//using DevExtreme.AspNet.Data; // of no use for dxScheduler
//using Newtonsoft.Json; // of no use for dxScheduler

namespace Infrastructure.ClientEntities
{
    public class ClientReportSchedule
    {
        public virtual Guid AccountId { get; set; }

        public virtual Guid ScheduleId { get; set; }

        public virtual IEnumerable<ClientReportScheduleSite> SiteIds { get; set; }

        public virtual Guid ReportId { get; set; } // Guid? - TBD

        public virtual string StartDate { get; set; }

        public virtual string EndDate { get; set; }

        public virtual byte ReportPeriod { get; set; }

        public virtual string Text { get; set; }
        // this is really ScheduleTitle (i.e. Appointment Title), but calling "Text" to match dxScheduler

        public virtual string EmailList { get; set; }

        public virtual string RecurrenceRule { get; set; }

        public virtual DateTime? LastSent { get; set; }

        public virtual DateTime? NextSend { get; set; }
    }
}